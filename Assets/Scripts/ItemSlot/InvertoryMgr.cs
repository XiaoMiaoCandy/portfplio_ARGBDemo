using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 用来管理物品栏
/// </summary>
public class InvertoryMgr : MonoBehaviour
{
    #region 单例模式
    private static InvertoryMgr instance;
    public static InvertoryMgr Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion


    //用来管理物品道具 和金币
    //玩家拥有的金币数
    public int goldCount = 0;
    public TMP_Text goldCountText;
    //所有的物品栏
    public ItemSlot[] itemSlots;
    private void Start()
    {
        goldCountText.text = goldCount.ToString();
    }

    #region 获取物品方法
    public void PickUpItem(ItemObject pickItem,ConsumableItem pickRealItem,int pickCount)
    {
        //金币的道具编号为 0
        if (pickItem.itemID == 0)
        {
            goldCount += pickCount;
            print(pickItem.itemID);
            goldCountText.text = goldCount.ToString();
            return;
        }
        else
        {
            Debug.Log("开始存储道具");
            //物品啦中是否存在该物品
            foreach (ItemSlot slot in itemSlots)
            {
                if (slot.ItemObject == null)
                {
                    print("该物品栏为空");
                    continue;
                    //跳过这轮循环，检查下一个物品栏是否为该物品
                }
                if (pickItem.itemID == slot.ItemObject.itemID )
                {
                    Debug.Log("物品栏有该物品，进行存储");
                    //且该物品栏没有被装满
                    if (slot.itemCount < slot.ItemObject.itemMaxStackable_Count)
                    {
                        if (slot.itemCount + pickCount > slot.ItemObject.itemMaxStackable_Count)
                        {
                            slot.itemCount = slot.ItemObject.itemMaxStackable_Count;
                            pickCount -= (slot.ItemObject.itemMaxStackable_Count - slot.itemCount);
                            PickUpItem(pickItem, pickRealItem, pickCount);
                            
                        }
                        //=========================
                        else
                            slot.itemCount += pickCount;//没有超过则正常相加
                        slot.UpdateItemSlot();
                        return;
                    }
                }
            }
            //是否存在空的物品栏
            foreach (ItemSlot slot in itemSlots)
            {
                Debug.Log("开始寻找空物品栏进行存储");
                if (slot.ItemObject == null)
                {
                    //发现空物品栏,进行存储
                    slot.ItemObject = pickItem;
                    slot.realItem = pickRealItem;
                    slot.itemCount = pickCount;
                    Debug.Log("存储成功：" + pickItem.itemNmae);
                    //存储完成，进行更新显示
                    slot.UpdateItemSlot();
                    return;
                }
            }
           
        }

        //运行到这里 表示 没有地方可以存储了
        DropItem(pickItem, pickCount);
    }
    #endregion

    #region 使用物品方法
    public void UseItem(ItemSlot targetItemSlot)
    {
        if (targetItemSlot == null || targetItemSlot.ItemObject == null)
        { 
            Debug.Log("使用攻击失败，存在空值 标识为1");
            return;
        }
        // 只有消耗品 且 数量大于0才能用
        if (targetItemSlot.ItemObject.itemType != ItemType.Consumable || targetItemSlot.itemCount <= 0)
        {
            Debug.Log("使用攻击失败，存在空值 标识为2");
            return;
        }
        if (targetItemSlot.realItem != null)
        {
            print("使用道具" + targetItemSlot.ItemObject.itemNmae);
            targetItemSlot.realItem.itemEffect?.Invoke();
            //物品被使用了，数量减少
            targetItemSlot.itemCount--;
        }
        if (targetItemSlot.itemCount <= 0)
        {
            targetItemSlot.ItemObject = null;
            targetItemSlot.itemCount = 0; 
            targetItemSlot.realItem = null; 
        }
        //更新物品栏
        targetItemSlot.UpdateItemSlot();
    }
    #endregion

    #region 丢弃物品
    public Transform playerTransform;
    
    public void DropItem(ItemSlot itemSlot, int dropCount)
    {
        if (itemSlot.realItem == null)
        {
            return;
        }
        ConsumableItem dropItem = Instantiate(itemSlot.realItem,playerTransform.position,Quaternion.identity).GetComponent<ConsumableItem>();
        dropItem.nums = dropCount;
        dropItem.gameObject.SetActive(true);
        dropItem.canPickUp1 = false;
        itemSlot.ItemObject = null;
        itemSlot.itemCount = 0;
        itemSlot.realItem = null;
        itemSlot.UpdateItemSlot();
    }
    private void DropItem(ItemObject drop,int dropCount)
    {
        ConsumableItem dropItem = Instantiate(drop.realItem1, playerTransform.position, Quaternion.identity).GetComponent<ConsumableItem>();
        dropItem.nums = dropCount;
        dropItem.canPickUp1 = false;
    }
    #endregion

    //进行查询所有的物品栏，查看物品栏中的物品是否是需求物品，且数量并未0
    public bool CheckItem(ItemObject item)
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if(slot.ItemObject == item&& slot.itemCount>0)
            {
                return true;
            }
        }
        return false;
    }
    //获取物品数量
    public int GetItemQuantity(ItemObject item)
    {
        int itemTotleAmount = 0;
        foreach(ItemSlot slot in itemSlots)
        {
            //遍历每一个物品槽
            if(slot.ItemObject == item)
            {
                Debug.Log("发现目标物品" + slot.ItemObject.itemNmae + " X " +slot.itemCount);
                //可能多个槽位都有该物品，所以进行累加
                itemTotleAmount += slot.itemCount;
            }
        }
        return itemTotleAmount;
    }

    #region 玩家获得金币方法
    public void AddGold(int gValue)
    {
        goldCount += gValue;
        //获得金币后，更新显示
        goldCountText.text = goldCount.ToString();
    }
    #endregion
}
