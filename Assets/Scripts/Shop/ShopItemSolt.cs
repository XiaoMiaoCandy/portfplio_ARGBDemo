using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 商品栏
/// </summary>
public class ShopItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    //商品
    public ShopItem shopItem;
    public ItemObject shopItemO;
    //商品名
    public TMP_Text shopItemName;
    //商品价格 显示
    public TMP_Text shopItemPrice;
    private int price;
    //商品图标
    public Image shopItemIcon;

    //进行商品初始化
    public void Initialize(ItemObject newItem,int newItemPrice)
    {
        shopItemO = newItem;
        shopItemIcon.sprite = shopItemO.itemSprite;
        shopItemName.text = shopItemO.itemNmae;
        price = newItemPrice;
        shopItemPrice.text = price.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShopInfo s = this.GetComponent<ShopInfo>();
        s.ShowShopItemIInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopInfo s = this.GetComponent<ShopInfo>();
        s.HideShopItemIInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        ShopInfo s = this.GetComponent<ShopInfo>();
        s.MoveShopItemInfo();
    }
}
