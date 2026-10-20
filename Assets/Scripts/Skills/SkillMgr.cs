using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillMgr : MonoBehaviour
{
    //存在的所有技能槽
    public SkillButton[] skillButtons;

    //当前选择的技能
    public SkillData skillData;
    private static SkillMgr instance;
    public static SkillMgr Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //该场景不销毁，通过GameManger统一管理
        //DontDestroyOnLoad(gameObject);

    }

    #region 技能信息的显示
    //显示的技能图片,通过关联面板上的信息来进行修改
    public Image skillImage;
    public TMP_Text skillNameText,skillLevelText,skillDescriptionText;
    public void DisPlaySkillInfo()
    {
        
        skillImage.sprite = skillData.skillSprite;
        skillNameText.text = skillData.skillName;
        skillLevelText.text = "SKILL LEVEL :" + skillData.skillLevel;
        skillDescriptionText.text = "Description \n" + skillData.skillDes;
    }
    #endregion

    #region 技能点信息显示
    //技能点的话，应该通过玩家 单例获取，这里通过Inspector窗口进行设置
    //public int skillNowPoints;
    //public int skillMaxPoints;
    public TMP_Text skillPointText;

    //技能加点更新UI
    public void UpdatePointUI()
    {
        if(skillData==null)
            return;
        //进行加点
        UpdataSkillInfo();
        //显示技能描述
        DisPlaySkillInfo();
        skillPointText.text = "POINTS: " + Player.Instance.skillNowPoint + "/" + Player.Instance.skillMaxPoint;
    }


    //对技能进行加点，来更新面板
    public void UpdataSkillInfo()
    {
        //不满足技能加点的要求
        if (skillData == null || skillData.skillLevel == skillData.skillMaxLevel || Player.Instance.skillNowPoint <= 0)
        {
            print("不满足加点条件");
            return;
        }
              
        //技能加点前要查看是否满足前置条件
        if (skillData.preSkills.Length == 0  )
        {
            AddPoint();
            //skillData.preSkills.Length == 0 ,不存在前置条件可以直接激活
        }
        else if (skillData.preSkills.Length != 0 )
        {
            //需要前置技能激活，才可以加点
            foreach(SkillData skillData in skillData.preSkills)
            {
                //前置技能被激活，该技能可以加点
                if (skillData.isOpenLock)
                {
                    //前置技能被激活，该技能可以加点
                    AddPoint();
                    //跳出循环
                    break;
                }
            }
        }
    }
    private void AddPoint()
    {
        //通过当前选择的技能ID,来将该技能槽激活：变现为颜色恢复为原色，激活状态为true,并显示当前技能等级
        skillButtons[skillData.skillID-1].GetComponent<Image>().color = Color.white;
        //将技能激活
        skillData.isOpenLock = true;
        //显示当前技能等级
        skillButtons[skillData.skillID - 1].transform.GetChild(0).gameObject.SetActive(true);
        skillData.skillLevel++;
        //skillNowPoints--;//这里关联玩家时，要对其进行扣除
        Player.Instance.skillNowPoint--;
        //这里是为了的到选择的技能槽下面的 文本相关
        //第一个.GetChild(0) 得到的是文本整体，包含遮罩 和 文本
        //第二步.GetChild(0) 得到的是文本
        skillButtons[skillData.skillID - 1].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text =
            skillData.skillLevel.ToString();
        //更新面板
    }
    
    public void ResetSkillPoint()
    {
        //将所有加点清除
        foreach (SkillButton s in skillButtons)
        {
            s.GetComponent<Image>().color = Color.gray;
            s.mySkillData.skillLevel = 0;
            s.mySkillData.isOpenLock = false;
            s.transform.GetChild(0).gameObject.SetActive(false);
        }
       
        Player.Instance.skillNowPoint = Player.Instance.skillMaxPoint;
       
        skillPointText.text = "POINTS: " + Player.Instance.skillNowPoint + "/" + Player.Instance.skillMaxPoint;
        DisPlaySkillInfo();
    }
    #endregion

    #region 加点效果

    public bool canUseFire =false;
    public void UpdatePlayerStatus()
    {
        foreach (SkillButton s in skillButtons)
        {
            switch (s.mySkillData.skillID)
            {
                case 1:
                    //技能1 为属性加点
                    Player.Instance.maxHp += s.mySkillData.skillLevel * 5;
                    break;
                case 2:
                    //技能2 为属性加点
                    Player.Instance.attack += s.mySkillData.skillLevel * 5;
                    break;
                case 3:
                    //加点技能等于5时，解锁主动技能
                    canUseFire = (s.mySkillData.skillLevel >= 5) ? true : false;
                    print("技能的激活状态：" + canUseFire);
                    break;
            }
        }
    }
    #endregion
}
