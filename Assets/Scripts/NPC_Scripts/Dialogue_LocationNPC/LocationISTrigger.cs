using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本用来监听玩家是否到达该区域
/// </summary>
public class LocationISTrigger : MonoBehaviour
{
    public LocationSo nowLocationSo;   //当前关联的地区数据

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果是玩家进入，则将该区域数据记录
        if(collision.gameObject.CompareTag("Player"))
        {
            LocationTracker.Instance.RecordLocation(nowLocationSo);
            //防止反复进入
            Destroy(gameObject);    
        }
    }
}
