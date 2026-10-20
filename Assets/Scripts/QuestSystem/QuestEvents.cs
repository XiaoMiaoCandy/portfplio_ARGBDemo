using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestEvents 
{
    //进行任务接取请求的方法
    public static Action<QuestSo> OnQuestOfferReQuested;
    //进行任务完成请求的方法
    public static Action<QuestSo> OnQuestTurnInRequested;
    //用来检查任务是否完成
    public static Func<QuestSo,bool> IsQuestCompleted;
    //任务完成的响应事件，创建奖励道具，添加经验，添加金币
    public static Action<QuestSo> OnQuestCompleted;
}
