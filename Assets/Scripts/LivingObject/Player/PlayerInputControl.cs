using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    
    private float h;
    private float v;
    private bool isAim = false;
    private bool isOpenShop = false;
    void FixedUpdate()
    {
        //瞄准状态 不能移动 攻击
       if (isAim) 
        {
            return;
        }
        if (Player.Instance == null)
            return;//防止玩家被销毁后继续执行
        // 玩家的移动
        Player.Instance.PlayerMove(h,v);
    }
    void Update()
    {
        if (Player.Instance == null)
            return;//防止玩家被销毁后继续执行
        //移动行为
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //攻击行为
        if (Input.GetKeyDown(KeyCode.J) && !Player.Instance.isAttacking && !isAim)
        {
            //将状态改为攻击,且攻击时不能移动
            Player.Instance.StopPlayerMove();
            Player.Instance.ChangeEntityState(E_EntityState.Attack);
        }
       
        //打开技能面板
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GamePanel_UI.Instance.OpenSkillPanel();
        }

        
        //打开商店面板
        if (Input.GetKeyDown(KeyCode.F))
        {
           if(Player.Instance.nowMerchant == null)
                return;
           else
           {
                isOpenShop = !isOpenShop;
                Player.Instance.nowMerchant.OpenAndCloseShop(isOpenShop);
                ShopPannel.Instance.isOpenShop = isOpenShop;
           }
        }
        // 进入瞄准状态
        if (Input.GetKeyDown(KeyCode.K) && !Player.Instance.isAttacking && SkillMgr.Instance.canUseFire)
        {
            // 进入瞄准模式
            isAim = !isAim;
            GameObject aim = Player.Instance.transform.GetChild(1).gameObject;
            print("当前的瞄准状态为："+ isAim);
            if (isAim)
            {
                aim.SetActive(true);
                Player.Instance.StopPlayerMove();
            }
            else
            {
                aim.SetActive(false);
                Player.Instance.ResetMove();
            }
        }
        // 进行开火
        if (!Player.Instance.isAttacking && SkillMgr.Instance.canUseFire && isAim)
        {
            // 瞄准状态：允许 QE 切换
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Player.Instance.shootAnimationID--;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.Instance.shootAnimationID++;
            }

            Player.Instance.shootAnimationID = Mathf.Clamp(Player.Instance.shootAnimationID, 0, 4);
            // 瞄准显示
            Player.Instance.ShoutAim();
            // 鼠标左键确认发射
            if (Input.GetMouseButtonDown(0))
            {
                Player.Instance.isAttacking = true;
                Player.Instance.ChangeEntityState(E_EntityState.FarAttack);
            }

          
        }
    }

}
