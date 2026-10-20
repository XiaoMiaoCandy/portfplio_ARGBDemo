using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本用于配置 某一角色的说话内容以及恢复内容
/// </summary>
[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/newDialogueData")]
public class Specker_TestSo : ScriptableObject
{
    public DialogueData[] dialogueDatas;    //与该NPC进行交流的基础数据
    public DialogueOption[] dialogueOptions;//与NPC进行交流时的选项 => 不同选项对应不同的交流分支
    
    //与NPC进行交流需要满足的条件
    public SpeckerSo[] needSpokenNPCs;      //需要先交流的NPC
    public LocationSo[] needLocatedAeras;   //需要到达的区域
    public ItemObject[] neeedItemObjects;   //需要具有的道具

    //标记处理
    public bool removeAfterPlay;    //在完成后将其移除
    public List<Specker_TestSo> testSoList; //关联的对话数据 => 将在移除对话时，辅助移除所有关联对话

    //检测前置条件满足的方法
    public bool IsConditonMet()
    {
        //只要存在任意一个条件不满足，就返回false
        foreach(SpeckerSo speckerSo in needSpokenNPCs)
        {
            if(!DialogueHistoryTracker.Instance.CheckSpokenNPC(speckerSo))
            {
                //没有在历史交流列表里找到，条件不满足
                return false;
            }
        }
        foreach (LocationSo locationSo in needLocatedAeras)
        {
            if(!LocationTracker.Instance.CheckLocation(locationSo))
            {
                return false;
            }
        }
        foreach ( ItemObject item in neeedItemObjects)
        { 
            if(!InvertoryMgr.Instance.CheckItem(item))
            {
                return false;
            }
        }
        return true;
    }
}

//因为，玩家和NPC对话，一般会是一问一答的。
//这种时候，如果让玩家存储所有自己的对话，就会太复杂
//所以因该由NPC进行存储，这样无论是NPC的连续说话，还是玩家的回答，都可以通过Specker进行区分
[System.Serializable]
public class DialogueData
{
    public SpeckerSo owner;
    [TextArea(3,5)]
    public string dialogueText;
}


//NPC对话时的选项
[System.Serializable]
public class DialogueOption
{
    public string optionText;           //选项内容 => 用于显示选项上的文字
    public Specker_TestSo nextDidlogue; //该选项对应的对话数据
}

