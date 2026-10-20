using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPannel : MonoBehaviour
{
    //测试时商店默认开启
    public bool isOpenShop = false;
    #region 单例模式
    private static ShopPannel instance;
    public static ShopPannel Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        
    }
    #endregion

    public ShopManager shopManager;
}
