using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel_UI : MonoBehaviour
{
    private static GamePanel_UI instance;
    public static GamePanel_UI Instance => instance;
    private void Awake()
    {
        instance = this;
    }




    //需要动态修改的数值，其一次为生命、等级
    private TMP_Text[] textMeshPros;
    //经验值滑动条
    private Slider expSlider;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPros = GetComponentsInChildren<TMP_Text>();
        expSlider = GetComponentInChildren<Slider>();
        if(expSlider==null)
        {
            print("sdadas1");
        }
    }

    public void UpdataStatic_HpUI()
    {
        textMeshPros[0].text = "Hp: " + Player.Instance.currentHp + " / " + Player.Instance.maxHp;
    }
    public void UpdateStaitc_ExpUI()
    {
        textMeshPros[1].text = "Level: " + Player.Instance.level;
        expSlider.maxValue = Player.Instance.expToLevel;
        expSlider.value = Player.Instance.nowAllExp;
    }
    

    //技能面板默认不开启
    private bool isOpenSkillPaneStatus = false;
    
    public void OpenSkillPanel()
    {
        //打开面板时，刷新UI界面
        SkillMgr.Instance.UpdatePointUI();
        isOpenSkillPaneStatus = !isOpenSkillPaneStatus;
        if (isOpenSkillPaneStatus)
        {
            //打开状态
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            //面板关闭后，计算玩家属性
            SkillMgr.Instance.UpdatePlayerStatus();
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(isOpenSkillPaneStatus);
    }
}
