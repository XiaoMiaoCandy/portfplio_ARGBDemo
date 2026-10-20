using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    //按钮所依赖的技能
    public SkillData mySkillData;
    public void OnPointerClick(PointerEventData eventData)
    {
        //当点击按钮时，将技能管理器的激活技能，设置为该按钮依赖的技能
        SkillMgr.Instance.skillData = mySkillData;
        SkillMgr.Instance.DisPlaySkillInfo();
    }
}
