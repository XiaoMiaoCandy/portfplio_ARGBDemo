using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestSo;

public class QuestManager : MonoBehaviour
{
    Dictionary<QuestSo, Dictionary<QuestObject, int>> questProgress = new();

    //获取任务进度
    public string GetProgressText(QuestSo questSo,QuestObject target)
    {
        int nowAcomunt = GetNowProgressAmount(questSo,target);
        print("=======" + nowAcomunt);
        //这里获取到的是0，说明进度并没有更新
        if (nowAcomunt >= target.needAmount)
        {
            //当前的数量 大于 需求数量
            return "Quest Complete";    //任务完成
        }
        else if (target.targetItem != null)
        {
            //玩家目标中的 道具不为空，且数量未完成，则返回当前的
            return nowAcomunt + "/" + target.needAmount;
        }
        else
        {
            //任务内容未 到达地点 或 与NPC进行交流
            return "Quest In Progress";
        }
    }
    
    //更新任务进度逻辑
    public void UpdateObjectTargetProgerss(QuestSo questSo, QuestObject target)
    {
        //检测任务栏中是否有这个任务
        if (!questProgress.ContainsKey(questSo))
        {
            //没有这个任务
            questProgress[questSo] = new Dictionary<QuestObject, int>();
        }
        //选择任务栏中有这个任务了，接下来对任务进行设置
        var progressDictionary = questProgress[questSo]; //存储的对应任务中的完成进度
        int nowTargetProgressAmount = 0;
        //这里检测一下任务目标是否和物品有关
        if (target.targetItem != null)
        {
            //这里证明任务目标需要道具，所以将当前数量进度设置背包中该道具的数目
            nowTargetProgressAmount = Mathf.Clamp(InvertoryMgr.Instance.GetItemQuantity(target.targetItem), 0, target.needAmount);
        }
        else if (target.targetLocation != null && LocationTracker.Instance.CheckLocation(target.targetLocation))
        {
            //这里证明任务需要到达指定地点，且已经到了，直接设置当前任务目标为完成
            nowTargetProgressAmount = target.needAmount;
        }
        else if (target.targetNPC != null &&DialogueHistoryTracker.Instance.CheckSpokenNPC(target.targetNPC))
        {
            //这里证明任务需要与指定NPC进行对话，且在已经对话列表清单中找到目标NPC，直接设置当前任务目标为完成
            nowTargetProgressAmount = target.needAmount;
        }
        //将进度辞典更新
        progressDictionary[target] = nowTargetProgressAmount;
    }
    //更新任务进度的方法 => 依赖UpdateObjectTargetProgerss
    public int GetNowProgressAmount(QuestSo questSo,QuestObject target)
    {
        UpdateObjectTargetProgerss(questSo, target);
        if (questProgress.TryGetValue(questSo, out var targetDictionary))
        {
            //这里任务进度中存在questSo
            if(targetDictionary.TryGetValue(target,out int amount))
            {
                return amount;
            }
        }

        return 0;
    }

    public void AcceptQuest(QuestSo questSo)
    {
        //创建一个字典，包含任务目标和任务进度
        questProgress[questSo] = new Dictionary<QuestObject, int>();
        foreach(QuestObject target in questSo.questTargets)
        {
            //得到该任务的使用任务目标
            UpdateObjectTargetProgerss(questSo,target);
        }
    }

    
    public List<QuestSo> GetActiveQuests()
    {
        return new List<QuestSo> ( questProgress.Keys );
    }
    public bool IsQuestAccepted(QuestSo questSo)
    {
        return questProgress.ContainsKey(questSo);
    }


    //检查任务完成
    private void OnEnable()
    {
        QuestEvents.IsQuestCompleted += IsQuestComplete;
        QuestEvents.OnQuestCompleted += OnQuestCompletedHandle;
        
    }
    private void OnDisable()
    {
        QuestEvents.IsQuestCompleted -= IsQuestComplete;
        QuestEvents.OnQuestCompleted -= OnQuestCompletedHandle;
    }
    public bool IsQuestComplete(QuestSo inquestSo)
    {
        if(!questProgress.TryGetValue(inquestSo,out var progressDic))
        {
            return false;
        }
        else
        {
            foreach(var target in inquestSo.questTargets)
            {
                UpdateObjectTargetProgerss(inquestSo,target);
            }
            foreach (var target in inquestSo.questTargets)  //检查该任务的所有目标是否完成
            {
                Debug.Log("开始检查任务是否完成");
                if (progressDic[target]<target.needAmount)
                {
                    Debug.Log("发现未完成目标");
                    return false;
                }
            }
            Debug.Log("所有目标均已完成");
            return true;
        }
    }

    //任务完成的方法，这里实现发放道具奖励，使用事件统一完成
    public void CompleteQuestGetItemReward(QuestSo inquestSo)
    {
        questProgress.Remove(inquestSo);
        foreach(var reward in inquestSo.questRewards)
        {
            InvertoryMgr.Instance.PickUpItem(reward.reward, reward.reward.realItem1, reward.amount);
        }
    }
    //任务完成事件统一调用的方法
    public void OnQuestCompletedHandle(QuestSo questSo)
    {
        //发放道具
        CompleteQuestGetItemReward(questSo);
        //增加经验
        Player.Instance.AddExp(questSo.expAmount);
        //增加金币
        InvertoryMgr.Instance.AddGold(questSo.goldAmount);
        Debug.Log($"任务{questSo.name}完成，发放了{questSo.expAmount}经验、{questSo.goldAmount}金币，以及{questSo.questRewards.Count}个道具");
    }
}
