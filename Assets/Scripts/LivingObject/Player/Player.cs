using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    #region Player 玩家单例 及 初始化
    //玩家在全局唯一，所以为了方便UI等其他脚本调用里面的方法，哭作为单例模式
    private static Player instance;
    public static Player Instance => instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
      
    }
    protected override void Start()
    {
        base.Start();
        //获得玩家的刚体 和 精灵编辑器
        playerRigidbody = this.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    
    #endregion

    #region Player 玩家组件
    private Rigidbody2D playerRigidbody;//玩家挂载的刚体对象
    private SpriteRenderer playerSpriteRenderer;//玩家挂载的精灵管理器
    #endregion

    #region 玩家移动 和 攻击 相关属性
    [Header("玩家的移动攻击属性")]
    //玩家是否可以移动
    private bool IsMove = true;
    public float playerMoveSpeed = 5.0f;//玩家的移动速度
     //玩家的攻击位置
    public Transform attackPos;
    public Vector2 attackSize;
    //是否可以攻击
    public bool isAttacking = false;
    #endregion

    #region 玩家基础属性
    [Header("玩家的特有属性")]
    public int level = 1;
    public int expToLevel = 10;
    public int nowAllExp = 0;

    public int skillNowPoint = 10;
    public int skillMaxPoint = 10;
    #endregion

    #region 玩家移动方法
    public void PlayerMove(float h,float v)
    {
        if (!IsMove)
        {
            return;
        }
       
        if(Mathf.Abs(h)>0|| Mathf.Abs(v)>0)
        {
            //将状态改为Run
            ChangeEntityState(E_EntityState.Run);
        }
        else
        {
            //将状态改为待机
            ChangeEntityState(E_EntityState.Idle);
        }
        Vector2 palyerMoveDic = new Vector2(h , v ).normalized;
        playerRigidbody.velocity = palyerMoveDic * playerMoveSpeed;
        //当玩家向左移动时图像翻转
        if (h > 0)
        {
            //玩家存在正向移动，不进行翻转
            transform.localScale = new Vector3(1, 1, 1);
            //playerSpriteRenderer.flipX = false;
        }
        else if (h < 0)
        {
            //玩家存在反方向移动，进行翻转
            transform.localScale = new Vector3(-1, 1, 1);
           // playerSpriteRenderer.flipX = true;
        }

    }
   
    public void PlayerStopMove()
    {
        IsMove = false;
        ChangeEntityState(E_EntityState.Idle);
    }
    public void RePlayerMove()
    {
        IsMove = true;
       // ChangeEntityState(E_EntityState.Run);
    }
    #endregion

    //玩家当前交互的商人
    public Merchant nowMerchant;

    #region 玩家攻击方法 (近战)
    //玩家进行攻击，当动画播放到最大攻击时调用
    public void PlayerAttack_Start()
    {
        isAttacking = true;
        Collider2D[] targets;
        //这里设置攻击动画，当玩家攻击结束后，会自动退出到原状态
        targets = Physics2D.OverlapBoxAll(attackPos.position, attackSize, 0);
       
        foreach (Collider2D targetCollider in targets)
        {
            if (targetCollider.gameObject.CompareTag("Monster"))
            {
                LivingEntity target = targetCollider.gameObject.GetComponent<LivingEntity>();
                //怪物的受伤逻辑
                this.AttackOther(target);
                Debug.Log("打到怪物: " + target.name);
            }
        }
    }
    // 攻击动画播放完毕自动调用
    public void PlayerAttack_End()
    {
        isAttacking = false;
        IsMove = true;
        ChangeEntityState(E_EntityState.Idle);
    }
    //用来显示攻击范围
    private void OnDrawGizmos()
    {
        if (attackPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackSize);
    }

    //玩家攻击时会停止移动
    public void StopPlayerMove()
    {
        IsMove = false;
        playerRigidbody.velocity = Vector3.zero;
    }
    public void ResetMove()
    {
        IsMove = true;
    }
    #endregion

    #region 玩家攻击（远程）
    public GameObject arrow;
   //射击动画列表
    private AnimationClip[] shootAnims;
    public int shootAnimationID = 2;
    
    //瞄准
    public void ShoutAim()
    {    
        if (!isAttacking)
        {
            float direction = transform.localScale.x > 0 ? -1f :1f;
            Vector3 baseDirection = Quaternion.AngleAxis(45 * shootAnimationID, Vector3.forward) * Vector3.up;
            Vector3 finalDirection = new Vector3(baseDirection.x * direction, baseDirection.y, 0);
            transform.GetChild(1).position = attackPos.position + finalDirection * 3;
        }
    }


    public void PlayerFarAttackStart()
    {
        if (isAttacking)
        {
            Vector3 aimPos = Player.Instance.transform.GetChild(1).position;
            Vector3 direction = aimPos - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject bullet = Instantiate(arrow, attackPos.position, Quaternion.Euler(0, 0, angle));

            Arrow arrowScript = bullet.GetComponent<Arrow>();
            arrowScript.owner = this.transform;
            print("创建了一个剑");
        }
        
    }
    public void PlayerFarAttackEnd()
    {
        isAttacking = false;
        IsMove = true;
        ChangeEntityState(E_EntityState.Idle);
    }

    public override void ChangeEntityState(E_EntityState state)
    {
        if (nowState == state)
            return;
        nowState = state;
        //将所有动画关闭
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsFarAttack", false);
        animator.SetInteger("ShootDir", 2);
        switch (nowState)
        {
            case E_EntityState.Idle:
                {
                    animator.SetBool("IsIdle", true);
                    break;
                }
            case E_EntityState.Attack:
                {
                    animator.SetBool("IsAttack", true);
                    break;
                }
            case E_EntityState.Run:
                {
                    animator.SetBool("IsRunning", true);
                    break;
                }
            case E_EntityState.FarAttack:
                {
                    animator.SetBool("IsFarAttack", true);
                    animator.SetInteger("ShootDir", Player.Instance.shootAnimationID);
                    break;
                }
        }
    }

    #endregion

    #region 玩家受伤方法
    //玩家的受伤逻辑
    public void PlayerDamage(Transform damagePos,float damageValue)
    {
        //玩家受伤，更新玩家血量的UI
        GamePanel_UI.Instance.UpdataStatic_HpUI();
        IsMove = false;
        playerRigidbody.velocity = (transform.position - damagePos.position).normalized * Mathf.Clamp(damageValue,0,5);
        StartCoroutine("PlayerDamage2");
    }
    IEnumerator  PlayerDamage2()
    {
        yield return new WaitForSeconds(1);
        playerRigidbody.velocity = Vector3.zero;
        IsMove = true;
        CheckDead();
    }
    #endregion

    #region 玩家升级方法
    public void AddExp(int eValue)
    {
        nowAllExp += eValue;
        if (nowAllExp >= expToLevel)
        {
            level++;
            nowAllExp -= expToLevel;
            expToLevel = Mathf.RoundToInt(expToLevel * 1.5f);
            print("玩家升级，当前经验：" +  nowAllExp);
            print("距离升级需要经验：" + (expToLevel-nowAllExp));
            skillMaxPoint += 2;
            skillNowPoint += 2;
        }
        //获得经验后更新经验UI
        GamePanel_UI.Instance.UpdateStaitc_ExpUI();
    }
    #endregion
}
