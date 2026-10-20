using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 该脚本是NPC对话的管理器
/// </summary>
public class DialogueMgr : MonoBehaviour
{
    #region 单列模式
    private static DialogueMgr instance;
    public static DialogueMgr Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if(instance != this)
                Destroy(gameObject);
        }
    }
    #endregion

    //UI显示关联
    [Header("UI相关")]
    public Image npcSprite;         //NPCd的精灵图 => 显示精灵图像
    public TMP_Text npcName;        //NPC的名字 => 用来显示在头像上，表明说话的对象
    public TMP_Text nowShowInfo;    //当前显示的内容 => 交流时的说的内容

    //交流内容
    
    private Specker_TestSo nowSpeckInfo;    //当前对话的数据 => 包含基础对话，选项对话
    //dialogueDatas ==> 基础对话内容  dialogueOptions == 分支对话内容
    private int nowSpeckInfoID;             //当前对话的内容索引
    //在一个nowSpeckInfo数据中包含多条对话，通过nowSpeckInfoID来索引具体显示的内容 ==> nowShowInfo
    public bool isSpecking = false;         //当前是否正在讲话


    //分支交流内容
    //在分支交流时，会有多个不同的选项
    public Button[] buttons;    //分支对话时的选项
    private void Start()
    {
        //在正常对话时，不需要选项按钮
        ClearChoices();
    }

    #region 更新显示
    //UI更新的方法 => 用来显示对话的内容
    private void UpdateInfoOnUI()
    {
        ClearChoices();
        //oneDialogueData 当前对话的某一条信息,其中包含 说话者 和说话内容
        DialogueData oneDialogueData = nowSpeckInfo.dialogueDatas[nowSpeckInfoID];

        //与NPC进行交流时将其添加到历史交流NPC中
        DialogueHistoryTracker.Instance.RecordSpokenNpc(oneDialogueData.owner);

        //将 精灵图 和 名字 赋值到UI对应的组件上
        npcSprite.sprite = oneDialogueData.owner.speckerSprite;
        npcName.text = oneDialogueData.owner.speckerName;
        //将 该条对话内容 赋值到UI对应的组件上
        nowShowInfo.text = oneDialogueData.dialogueText;
        nowSpeckInfoID++;
    }
    //UI更新方法 => 用来显示选项按钮上的信息
    private void ShowOptionInfo()
    {
        ClearChoices();
        if (nowSpeckInfo.dialogueOptions.Length != 0)
        {
            //存在选项，则遍历所有选项，并将其显示出来
            //这里需要button和对应的选项信息是同索引
            for (int i = 0; i < nowSpeckInfo.dialogueOptions.Length; i++) 
            {
                //选项框激活
                buttons[i].gameObject.SetActive(true);
                //从讲话的信息中提取选项信息
                DialogueOption option = nowSpeckInfo.dialogueOptions[i];
                //将选项框的文本赋值为 具体的选项信息 
                //这里的文本框位于子对象上
                buttons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                //为对应的选项 添加事件响应
                buttons[i].onClick.AddListener(()  => ChooseOPtion(option.nextDidlogue));
            }
        }
    }
    //UI更新方法 => 用于清除选项的残留 (这里可以只要存在UI更新就先清理一下，防止出现残留)
    private void ClearChoices()
    {
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
    #endregion

    #region 交流方法
    //NPC开始进行交流的方法 => 通过玩家的交流状态，传入具体的交谈内容
    public void StartDialogue(Specker_TestSo specker_Info)
    {
        nowSpeckInfo = specker_Info;
        nowSpeckInfoID = 0;

        UpdateInfoOnUI();
        //NPC正在进行交流
        isSpecking = true;
    }
    //NPC继续进行交流的方法 => 当玩家处于交流状态时，且还存在没有说的话，则进行继续交流
    public void StayDialogue()
    {
        //保持交流
        if(nowSpeckInfoID<nowSpeckInfo.dialogueDatas.Length)
        {
            UpdateInfoOnUI();
        }
        else
        {
            //结束对话，关闭对话UI =(更新)=>对话结束后检测有无分支对话
            if (nowSpeckInfo.dialogueOptions.Length > 0)
            {
                ShowOptionInfo();
            }
            else
            {
                EndDialogue();
                Debug.Log("对话结束");
            }
        }
    }
    //NPC结束进行交流的方法 => 当NPC结束交流时，需要将对话框界面隐藏，这里可以使用失活或者将透明度设置为0
    public void EndDialogue()
    {
        //当前交流数据读完了，重置数据
        nowSpeckInfoID = 0;
        isSpecking = false;
        this.gameObject.SetActive(false);
    }
    
    //在进行分支对话时，通过选择不同的分支实习下一个新的对话内容的差异
    public void ChooseOPtion(Specker_TestSo nextDidlogue)
    {
        if (nextDidlogue == null)
        {
            //不存在后续分支，这个时候意味着对话彻底结束
            EndDialogue();
        }
        else
        {
            //反之则开启一个新的对话
            StartDialogue(nextDidlogue);
        }
    }

    
    #endregion
}
