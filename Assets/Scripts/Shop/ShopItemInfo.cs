using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 用来介绍商品的信息
/// 挂载在物品栏上
/// </summary>
public class ShopInfo : MonoBehaviour
{
    private ShopItemSlot slot;
    private ItemObject item;

    public TMP_Text nowItemName;
    public TMP_Text nowItemDesc;
    public GameObject infoItemPanel;
    void Awake()
    {
        slot = GetComponent<ShopItemSlot>();
        if(slot.shopItemO != null )
        {
            print("不等于null");
            item = slot.shopItemO;
        }
        else
        {
            return;
        }
    }
    #region 信息介绍
    public void ShowShopItemIInfo()
    {
        if (slot != null && slot.shopItemO != null)
        {
            item = slot.shopItemO;
            infoItemPanel.SetActive(true);
            nowItemName.text = item.itemNmae; 
            nowItemDesc.text = item.itemInfo;
        }
    }
    public void HideShopItemIInfo() 
    {
        nowItemName.text = "";
        nowItemDesc.text = "";
        infoItemPanel.SetActive(false); 
    }

    public void MoveShopItemInfo()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 offset = new Vector3(10, -10, 0);
        RectTransform rectTransform =infoItemPanel.GetComponent<RectTransform>();
        rectTransform.position = mousePos + offset;
    }
    #endregion
}
