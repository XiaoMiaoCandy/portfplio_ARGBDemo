using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class QuestRewardSlot : MonoBehaviour
{
    //奖励UI相关
    public Image rewardImage;       //该奖励的道具对应的图片
    public TMP_Text rewardAmount;   //该奖励的道具对应的数量


    public void UpdateUIONRewardSlot(Sprite sprite, int amount)
    {
        //用来更新奖励槽位的UI
        rewardImage.sprite = sprite;
        rewardAmount.text = amount.ToString();

    }

}
