using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTracker : MonoBehaviour
{
    #region 单例模式
    private static LocationTracker instance;
    public static LocationTracker Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
    #endregion

    readonly List<LocationSo> locations = new(); //存储所有到达过的地区

    //记录玩家到达的地区
    public void RecordLocation(LocationSo location)
    {
        if (location == null)
        {
            Debug.Log("没有交流的人");
            return;
        }
        if (!CheckLocation(location))
        {
            //列表中不存在这个地区则将其添加进去
            locations.Add(location);
            Debug.Log("玩家到达了 " + location.locationName);
        }
    }
    //用于检测某以地区是否在列表中
    public bool CheckLocation(LocationSo location)
    {
        return locations.Contains(location);
    }
}
