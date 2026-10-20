using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static QuestSo;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;

    //引用UI面板组件
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private TMP_Text rewardExpAmount;   //该奖励对应的经验数量
    [SerializeField] private TMP_Text rewardGoldAmount;  //该奖励对应的金币数量
    [SerializeField] private Button closeButton;    //关闭任务面板按钮

    [SerializeField] private QuestTargetSlot[] questTargetSlots;    //该任务的所有任务目标的槽位
    [SerializeField] private QuestRewardSlot[] questRewardSlots;    //该任务的所有任务奖励的槽位
    [SerializeField] private QuestLogSlot[] questLogSlots;  //任务槽位
    private QuestSo nowQuest;
    [SerializeField] private QuestSo noQuest;
    
    //待处理的任务（玩家查看过但还没接受/拒绝的任务）
    private QuestSo pendingQuest;


    private void OnEnable()
    {
        //在脚本启用时使用
        QuestEvents.OnQuestOfferReQuested += ShowQuestOffer;
        closeButton.onClick.AddListener(OnDelineQuestClicked);
    }
    private void OnDisable()
    {
        //在脚本启用时使用
        QuestEvents.OnQuestOfferReQuested -= ShowQuestOffer;
    }


    //获取任务画布
    [SerializeField] private CanvasGroup questGroup;
    [SerializeField] private CanvasGroup acceptGroup;
    [SerializeField] private CanvasGroup delineGroup;
    [SerializeField] private CanvasGroup completeGroup;
    private void SetCanvasGroupState(CanvasGroup gruop,bool activate)
    {
        gruop.alpha = activate ? 1 : 0;
        gruop.blocksRaycasts = activate;
        gruop.interactable = activate;
    }
    public void ShowQuestOffer(QuestSo incomingQuest)
    {
        List<QuestSo> activeQuests = questManager.GetActiveQuests();
        
        //先记录传入的任务作为待处理任务
        pendingQuest = incomingQuest;
        
        //优先检查是否有已接受的任务
        if (activeQuests.Count > 0)
        {
            nowQuest = activeQuests[0];
            SetCanvasGroupState(acceptGroup, false);  //已接受任务不显示接受按钮
            SetCanvasGroupState(delineGroup, false);
            
            //检查任务是否完成，如果完成则显示完成按钮
            if (questManager.IsQuestComplete(nowQuest))
            {
                SetCanvasGroupState(completeGroup, true);
            }
            else
            {
                SetCanvasGroupState(completeGroup, false);
            }
        }
        //如果没有已接受任务，但有待处理任务（新任务）
        else if (pendingQuest != null)
        {
            nowQuest = pendingQuest;
            SetCanvasGroupState(acceptGroup, true);   //新任务显示接受按钮
            SetCanvasGroupState(delineGroup, true);
            SetCanvasGroupState(completeGroup, false);
        }
        //没有任何任务
        else
        {
            nowQuest = noQuest;
            SetCanvasGroupState(acceptGroup, false);
            SetCanvasGroupState(delineGroup, false);
            SetCanvasGroupState(completeGroup, false);
        }
        
        HandleQuestCliked(nowQuest);
        SetCanvasGroupState(questGroup, true);
    }

    //点击接受任务的方法
    public void OnAcceptQuestClicked()
    {
        if (pendingQuest != null)
        {
            questManager.AcceptQuest(pendingQuest);
            pendingQuest = null;  //清除待处理任务
        }
        SetCanvasGroupState(questGroup, false);
        RefreshQuestList();
    }
    
    //点击拒绝任务的方法
    public void OnDelineQuestClicked()
    {
        pendingQuest = null;  //清除待处理任务
        SetCanvasGroupState(questGroup, false);
    }
    //点击完成任务的方法
    public void OnCompleteQuestClicked()
    {
        //检查任务是否完成
        if (questManager.IsQuestComplete(nowQuest))
        {
            //触发任务完成事件，QuestManager 会自动发放奖励
            QuestEvents.OnQuestCompleted?.Invoke(nowQuest);
            //清除待处理任务
            pendingQuest = null;
            //关闭任务面板
            SetCanvasGroupState(questGroup, false);
            //刷新任务列表
            RefreshQuestList();
            Debug.Log($"任务 {nowQuest.questName} 完成，已发放奖励");
        }
        else
        {
            Debug.Log("任务尚未完成，无法领取奖励");
        }
    }
    public void RefreshQuestList()
    {
        List<QuestSo> list = questManager.GetActiveQuests();
        for (int i = 0; i < questLogSlots.Length; i++)
        {
            if(i<list.Count)
            {
                questLogSlots[i].SetQuestSlot(list[i]);
            }
            else
            {
                questLogSlots[i].ClearQuestSlot();
            }
        }
    }


    //用来更新任务描述的界面，包含更新任务目标，任务奖励
    public void HandleQuestCliked(QuestSo questSo)
    {
        this.nowQuest = questSo;
        questNameText.text = questSo.questName;
        questDescriptionText.text = questSo.questDescription;
        DisPlayQuestTarget();
        DisPlayQuestReward();
    }
    //更新nowQuest中的所有目标
    private void DisPlayQuestTarget()
    {

        for (int i = 0; i < questTargetSlots.Length; i++)
        {
            if(i<nowQuest.questTargets.Count)
            {
                //取出当前任务目标
                var target = nowQuest.questTargets[i];
                //获取当前进度值
                int nowAmount = questManager.GetNowProgressAmount(nowQuest, target);
                //获取当前进度的完成情况
                string progress = questManager.GetProgressText(nowQuest,target);
                bool isComplete = nowAmount>target.needAmount;
                //将当前任务目标栏设置为显示
                questTargetSlots[i].gameObject.SetActive(true);
                //更新当前任务目标 的UI显示
                questTargetSlots[i].RefreshTarget(target.description,progress,isComplete);
            }
            else
            {
                questTargetSlots[i].gameObject.SetActive(false);
            }
        }
    } 
    private void DisPlayQuestReward()
    {
        for(int i = 0;i < questRewardSlots.Length;i++)
        {
            //查找所有的任务奖励槽位,这个时候任务奖励槽位是空的
            if(i<nowQuest.questRewards.Count)
            {
                //该任务中存在任务奖励，取出当前的任务奖励
                var nowReward = nowQuest.questRewards[i];
                //将当前任务槽位打开
                questRewardSlots[i].gameObject.SetActive(true);
                //更新该槽位的UI
                questRewardSlots[i].UpdateUIONRewardSlot(nowReward.reward.itemSprite, nowReward.amount);

            }
            else
            {
                //没有找到，则将该槽位关闭
                questRewardSlots[i].gameObject.SetActive(false);
            }
        }
        //更新金币及经验数目
        rewardExpAmount.text = "EXP: " + nowQuest.expAmount;
        rewardGoldAmount.text = "Gold: " + nowQuest.goldAmount;

    }


}
