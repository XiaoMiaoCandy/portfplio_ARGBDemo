
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="QuesteSo",menuName ="New QuestSo")]
public class QuestSo : ScriptableObject
{
    public string questName;        //任务名称
    public int questLevel;          //任务等级
    [TextArea(3,5)]
    public string questDescription; //任务描述
    public List<QuestObject> questTargets;  //该任务要完成的目标
    public List<QuestReward> questRewards;  //该任务的奖励清单

    //任务的基础奖励
    public int expAmount;       //奖励经验值的数量
    public int goldAmount;      //奖励金币的数量

    [System.Serializable]
    public class QuestObject
    {
        //任务描述
        public string description;
        //游戏目标容器，用来存储具体的任务目标
        [SerializeField] private Object questTarget;
        //任务目标属性
        public SpeckerSo targetNPC => questTarget as SpeckerSo;
        public ItemObject targetItem => questTarget as ItemObject;
        public LocationSo targetLocation => questTarget as LocationSo;
        //任务进度追踪
        public int needAmount;  //需要的数量
        public int nowAmount;   //当前的数量
    }


    [System.Serializable]
    public class QuestReward
    {
        //任务奖励
        public ItemObject reward;   //道具奖励
        public int amount;          //奖励道具的数量
    }


}
