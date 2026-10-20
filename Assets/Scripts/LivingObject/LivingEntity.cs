using UnityEngine;

public enum E_EntityState
{
    Idle,
    Run,
    Attack,
    FarAttack,
    //没有死亡动画
    //Dead
}
public class LivingEntity : MonoBehaviour
{


    [Header("基础属性")]
    // 姓名、最大血量、当前血量、是否死亡、攻击力、防御力
    public string objectName = "example01";
    public int maxHp = 100;
    public int currentHp = 100;
    public bool isDead = false;
    public int attack = 10;
    public int defence = 5;


    protected Animator animator;
    protected E_EntityState nowState;

    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
       
    }


    /// <summary>
    /// 用来进行血量变换的控制，加血value为正数，反之为扣血
    /// </summary>
    /// <param name="value"></param>
    public virtual void ChangeHp(int changeValue)
    {
        currentHp = Mathf.Clamp(changeValue + currentHp, 0, maxHp);
        print("生命发生改变" + currentHp);
    }
    public virtual void AttackOther(LivingEntity attackTarget)
    {
        //死亡后不能继续鞭尸
        if (isDead)
        {
            return;
        }
        // 伤害公式：攻击 - 防御，最低为 1
        int damage = -Mathf.Max(attack - attackTarget.defence, 1);
        attackTarget.ChangeHp(damage);//可能是玩家，也可能是怪物
       
    }
    public virtual void CheckDead()
    {
        //玩家没有死亡时，调用防止鞭尸
        if (currentHp <=0&&!isDead)
        {
            isDead = true;
            Destroy(gameObject);
            //ChangeEntityState(E_EntityState.Dead);
        }
    }

    public virtual void ChangeEntityState(E_EntityState state)
    {
        if (nowState == state) 
            return;
        nowState = state;
        //将所有动画关闭
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", false);
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
        }
    }
}

