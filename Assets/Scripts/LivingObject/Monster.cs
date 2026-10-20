using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    private Rigidbody2D rb;

    //怪物的移动方式为，在几个点上 随机移动
    [Header("移动相关")]
    //移动速度
    public float moveSpeed = 3;
    //巡逻的范围,默认在这两点巡逻
    public Vector3[] rangePos = { Vector3.left, Vector3.right * 10 };
    private Vector3 nowTargetPos;
    //当前怪物的目标及目标位置 
    private Transform target;
    //是否处于进攻状态，在进攻状态时，怪物会追踪玩家，直到玩家离开视野
    private bool isAttack = false;
    private int index = 0;

    [Header("怪物攻击相关")]
    //怪物的攻击位置
    public Transform attackPos;
    //怪物的攻击范围
    public Vector2 attackSize;
    //怪物的攻击距离
    public float attackDistance;
    //攻击冷却
    public float attackCoolTime = 1f;
    private float isAttackTime = 1f;

    [Header("怪物死亡掉落的道具")]
    public UnityEngine.Object[] objects;
    public int expValue = 5;

    //怪物死亡，会引起玩家相关的变化，如增加经验，用来进行怪物死亡后的操作
    public event Action OnDead;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        base.Start();
        nowTargetPos = rangePos[index];

        // 单例自动获取玩家，不用你手动拖了
        target = Player.Instance.transform;
        //为怪物死亡添加死亡后会出现的变化
        OnDead += HandleDeadEffect;
    }

    private void FixedUpdate()
    {
        if (isAttackTime == 0)
        {
            MonsterMove();
        }
    }
    private void Update()
    {
        if (isAttackTime > 0)
            isAttackTime = Mathf.Clamp01(isAttackTime - Time.deltaTime);
        CheckDead();

    }
    public void MonsterMove()
    {
        // 目标在右侧
        if (nowTargetPos.x > transform.position.x && transform.localScale.x < 0 ||
            nowTargetPos.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }


        //是否为进攻状态
        if (isAttack)
        {
            Vector2 direction = (nowTargetPos - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            if (Vector3.Distance(transform.position, nowTargetPos) < 0.5)
            {
                index++;
                nowTargetPos = rangePos[index % rangePos.Length];
            }
            Vector2 direction = (nowTargetPos - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        ChangeEntityState(E_EntityState.Run);
    }

    public void MonsterAttack()
    {

        Collider2D[] targets;
        targets = Physics2D.OverlapBoxAll(attackPos.position, attackSize, 0);
        foreach (Collider2D targetCollider in targets)
        {
            if (targetCollider.gameObject.CompareTag("Player"))
            {
                //单例直接获取玩家，不用GetComponent
                Player target = Player.Instance;

                //玩家的受伤逻辑
                this.AttackOther(target);
                target.PlayerDamage(attackPos, Mathf.Max(attack - target.defence, 1));
                Debug.Log("打到玩家: " + target.name);
            }
        }
        //攻击结束站立
        ChangeEntityState(E_EntityState.Idle);


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = Player.Instance.transform;
            nowTargetPos = target.position;
            if (Vector3.Distance(transform.position, nowTargetPos) < attackDistance && isAttackTime == 0)
            {
                //开始攻击，计算攻击冷却
                isAttackTime = attackCoolTime;
                rb.velocity = Vector3.zero;
                //当玩家进入可攻击范围，进行攻击
                ChangeEntityState(E_EntityState.Attack);
            }
        }
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttack = false;
            rb.velocity = Vector2.zero;
        }
    }


    public override void CheckDead()
    {
        base.CheckDead();
        if(isDead)
            OnDead();
    }
    //怪物死亡效果
    private void HandleDeadEffect()
    {
        //通知玩家增加经验
        Player.Instance.AddExp(expValue);
        ////通知UI改变经验值
        //GamePanel_UI.Instance.UpdateStaitc_ExpUI();
    }
}