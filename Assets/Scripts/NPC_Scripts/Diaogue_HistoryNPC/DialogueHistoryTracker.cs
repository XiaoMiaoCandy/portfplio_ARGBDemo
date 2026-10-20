using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///该脚本为单例模式，用来存储与玩家交谈过的NPC对象
/// </summary>
public class DialogueHistoryTracker : MonoBehaviour
{
    #region 单例模式
    private static DialogueHistoryTracker instance;
    public static DialogueHistoryTracker Instance=> instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
    #endregion

    readonly List<SpeckerSo> spokenNPCs = new(); //存储所有交流过的npc => 用来后续检测有无进行交流

    //记录玩家交流的NPC对象
    public void RecordSpokenNpc(SpeckerSo spokenNPC)
    {
        if (spokenNPC == null)
        {
            Debug.Log("没有交流的人");
            return;
        }
        if (!CheckSpokenNPC(spokenNPC))
        {
            //列表中不存在这个NPC则将其添加进去
            spokenNPCs.Add(spokenNPC);
            Debug.Log("刚刚我进行了交流与 "+ spokenNPC.speckerName);
            //前置条件可以可视化一个另一个对象来充当未知角色，只有满足前置条件
            //才会将NPC转为真正的角色
        }
    }
    //用于检测交流过的NPC是否在列表中
    public bool CheckSpokenNPC(SpeckerSo spokenNPC)
    {
        //我不希望玩家 和未知角色也添加到 历史交流对象中去
        if (spokenNPC.speckerName == "Player" || spokenNPC.speckerName == "???")
            return true;
        return spokenNPCs.Contains(spokenNPC);
    }
}
