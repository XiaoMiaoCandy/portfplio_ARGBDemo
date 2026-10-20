using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 这是技能数据模板，用来通过Inpspector窗口来调整不同技能的参数
/// </summary>

// 
[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillData : ScriptableObject
{
    [Header("固定属性")]
    public int skillID;
    public string skillName;
    public Sprite skillSprite;
    [TextArea(1,8)]
    //技能描述，可以让技能信息在Inspector窗口多行显示
    public string skillDes;
    //技能的当前等级
    public int skillLevel;
    //技能的最大等级
    public int skillMaxLevel;
    //技能是否被解锁
    public bool isOpenLock;//作用：用来判断后续技能是否可以被解锁
    //解锁该技能，需要激活的技能
    public SkillData[] preSkills;
}
