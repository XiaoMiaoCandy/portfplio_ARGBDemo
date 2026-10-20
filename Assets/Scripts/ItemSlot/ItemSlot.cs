using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //道具的数据信息
    public ItemObject ItemObject;
    //道具关联的预设体
    public ConsumableItem realItem;
    //道具的数量
    public int itemCount;

    //道具UI相关
    public Image itemImage;
    public TMP_Text itemTextCount;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //点击道具，使用道具
            InvertoryMgr.Instance.UseItem(this);
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (ShopPannel.Instance == null)
            {
                InvertoryMgr.Instance.DropItem(this, this.itemCount);
                return;
            }
            if (ShopPannel.Instance.isOpenShop)
            {
                ShopPannel.Instance.shopManager.SellShopItem(this);
                UpdateItemSlot();
            }
          
        }
    }

    public void UpdateItemSlot()
    {
        if(ItemObject != null)
        {
            //将物品栏UI显示
            itemImage.gameObject.SetActive(true);
            itemTextCount.gameObject.SetActive(true);
            //更新内容
            itemImage.sprite = ItemObject.itemSprite;
            itemTextCount.text = itemCount.ToString();

        }
        else
        {
            itemImage.sprite = null;
            itemTextCount.text = null;
            itemImage.gameObject.SetActive(false);
            itemTextCount.gameObject.SetActive(false);
        }
    }
}
