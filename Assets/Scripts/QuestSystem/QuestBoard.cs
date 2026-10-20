using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private QuestSo questSo;
    [SerializeField] private QuestSo questToTurnIn;
    private bool isRangeArea;

    private void OnEnable()
    {
        //监听任务完成事件，完成后清空任务以便再次接取
        QuestEvents.OnQuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestCompleted -= OnQuestCompleted;
    }

    private void OnQuestCompleted(QuestSo completedQuest)
    {
        //如果完成的是当前任务板提供的任务，清空任务
        if (questToTurnIn == completedQuest || questSo == completedQuest)
        {
            questSo = null;
            questToTurnIn = null;
            Destroy(gameObject,1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isRangeArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isRangeArea = false;
        }
    }

    private void Update()
    {
        if(questSo == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F) && isRangeArea)
        {
            Debug.Log("触发任务板");
            //玩家需要在面板中手动点击"完成"按钮来领取奖励
            QuestEvents.OnQuestOfferReQuested?.Invoke(questSo);
        }
    }
}