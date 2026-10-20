using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    //商店货物列表，相当于库存
    [SerializeField] public List<ShopItem> shopItems;
    //该商人的商店面板
    public ShopPannel shopPannel2;
    public bool canOpenShop = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpenShop = true;
            //展示商店图标
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Player.Instance.nowMerchant = this;
            //shopPannel.gameObject.SetActive(true);
            
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //隐藏商店图标
        if (collision.CompareTag("Player"))
        {
            canOpenShop = false;
            //展示商店图标
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            Player.Instance.nowMerchant = null;
        }
    }
    public void OpenAndCloseShop(bool isOpen)
    {
        shopPannel2.gameObject.SetActive(isOpen);
        if (isOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

}
