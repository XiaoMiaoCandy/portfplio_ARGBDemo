using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Merchant shopOwner;
    //商店货物列表，相当于库存
    private List<ShopItem> shopItems;
    //商店货架，商品可以上架到货架上
    [SerializeField] private ShopItemSlot[] shopItemSlot;
    void Start()
    {
        shopItems = shopOwner.shopItems;
        PopulateShopItems();
    }

    //商品上架的方法
    public void PopulateShopItems()
    {
        for(int i = 0; i < shopItems.Count && i<shopItemSlot.Length; i++)
        {
            //从库存里取出商品
            ShopItem loadItem = shopItems[i];
            //将商品上架到货架
            shopItemSlot[i].Initialize(loadItem.shopItemObject, loadItem.shopPrice);
            shopItemSlot[i].shopItem = loadItem;
        }
        //货品的数量小于 货架栏
        for (int i = shopItems.Count; i < shopItemSlot.Length; i++)
        {
            shopItemSlot[i].gameObject.SetActive(false);
        }
    }

    #region 购买商品方法
    public void BuyShopItem(ShopItemSlot item)
    {
        //检查购买的物品，和当前金币数量
        if(item != null&&InvertoryMgr.Instance.goldCount>=item.shopItem.shopPrice)
        {
            //满足购买条件
            InvertoryMgr.Instance.PickUpItem(item.shopItem.shopItemObject, item.shopItem.shopItemObject.realItem1,1);
            InvertoryMgr.Instance.goldCount -= item.shopItem.shopPrice;
            //花销金币进行UI更新
            InvertoryMgr.Instance.goldCountText.text = InvertoryMgr.Instance.goldCount.ToString();

        }
    }
    #endregion
    #region 出售商品方法
    public void SellShopItem(ItemSlot itemSlot)
    {
        if (itemSlot.ItemObject != null && itemSlot.itemCount >= 1)
        {
            InvertoryMgr.Instance.goldCount += Mathf.CeilToInt(itemSlot.ItemObject.itemValue * 0.75f);
            InvertoryMgr.Instance.goldCountText.text = InvertoryMgr.Instance.goldCount.ToString();
            itemSlot.itemCount--;
        }
        if (itemSlot.itemCount <= 0)
        {
            itemSlot.ItemObject = null;
            itemSlot.itemCount = 0;
            itemSlot.realItem = null;
        }
    }
    #endregion
}
