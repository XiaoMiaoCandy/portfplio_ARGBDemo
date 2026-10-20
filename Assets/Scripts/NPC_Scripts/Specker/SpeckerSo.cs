using UnityEngine;

/// <summary>
/// 该脚本用于实现创建Specker对象，并允许通过Inspector窗口快速配置Specker
/// </summary>
[CreateAssetMenu(fileName = "Specker",menuName ="Dialogue/newSpecker")]
public class SpeckerSo : ScriptableObject
{
    public string speckerName;      //说话的人的名字
    public Sprite speckerSprite;    //说话的人所拥有的精灵图
}

