using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLogSlot : MonoBehaviour
{
    //通过[SerializeField]，让私有变量可序列化。这里直接用Public不可以吗
    [SerializeField] private TMP_Text questNameText;    //任务的名称显示文本
    [SerializeField] private TMP_Text questLevelText;   //任务的等级显示文本

    public QuestSo nowQuest;    //该任务槽关联的任务对象
    public QuestLogUI questLogUI;

    
    private void OnValidate()
    {
        if (nowQuest != null)
            SetQuestSlot(nowQuest);
        else
            gameObject.SetActive(false);
    }

    public void SetQuestSlot(QuestSo questSo)
    {
        nowQuest = questSo;
        questNameText.text = questSo.questName;
        questLevelText.text = "Lv." + questSo.questLevel;
        gameObject.SetActive (true);
    }
    //清除已经完成的任务，或者拒绝的任务
    public void ClearQuestSlot()
    {
        nowQuest = null;
        gameObject.SetActive(false );
    }

    //当用户点击该槽位时调用的方法
    public void OnSlotClicked()
    {
        questLogUI.HandleQuestCliked(nowQuest);
    }
}
