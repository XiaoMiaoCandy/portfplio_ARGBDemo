using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    /// <summary>
    /// 材料类
    /// </summary>
    Material,
    /// <summary>
    /// 消耗品
    /// </summary>
    Consumable
}

/// <summary>
/// /道具的基本属性
/// </summary>
[CreateAssetMenu(fileName ="New Item")]
public class ItemObject : ScriptableObject
{
    #region 物品的基础属性
    // 道具的编号、名字、信息介绍、图像显示、价值
    [Header("基础属性")]
    public int itemID;
    public string itemNmae;
    [TextArea(3,5)]
    public string itemInfo;
    public Sprite itemSprite;
    public int itemValue;
    public ItemType itemType;//道具的类型
    [SerializeField]
    public ConsumableItem realItem1;
    #endregion

    #region 道具的背包存储相关属性
    //是否可叠加
    public bool isStackableItem = false;
    //最大可叠加数量
    public int itemMaxStackable_Count = 64;
    #endregion
}
