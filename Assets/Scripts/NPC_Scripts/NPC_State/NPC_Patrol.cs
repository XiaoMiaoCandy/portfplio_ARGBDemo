using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本为驱动NPC进行巡逻脚本
/// </summary>
public class NPC_Patrol : MonoBehaviour
{
    //移动参数相关 => 外部配置
    public Vector2[] patrolArrs;    //NPC进行巡逻的地点 => NPC将在这几个点中进行巡逻
    public float patrolSpeed = 2f;  //NPC巡逻时的移动速度
    public float pauseTime = 1.0f;  //NPC到达指定位置后要停止运动的时长
    private bool isPause = false;   //NPC是否要暂定运动 => 只有到达指定位置后会进行暂定

    //移动目标相关
    private Vector2 target;         //NPC巡逻的当前目标点 => 由patrolArrs决定
    private int targetIndex = 0;    //NPC巡逻时目标点索引 => 决定了NPC巡逻的下一个目标点
    
    //动画播放器和NPC刚体
    private Rigidbody2D rb;         //驱动NPC移动的脚本
    private Animator animator;      //驱动动画播放的组件

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //NPC的动画由NPC_Sprite驱动
        animator = GetComponentInChildren<Animator>();
        target = patrolArrs[targetIndex];
        targetIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPause)
        {
            //NPC暂停移动，播放待机动画
            animator.Play("Idle");
            rb.velocity = Vector2.zero;
            return;
        }
        animator.Play("Walk");
        //玩家移动的方向 => 进行归一化的原因说，防止方向向量模长的大小对 刚体移动速度的影响
        Vector2 direction = (target - (Vector2)this.transform.position).normalized;
        //这里进行玩家是否转向的处理
        if (direction.x < 0&&this.transform.localScale.x>0|| direction.x > 0 && this.transform.localScale.x < 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        //玩家的移动速度 => 通过修改刚体的速度 来驱动玩家移动
        rb.velocity = direction * patrolSpeed;
        //进行距离判断 => 当NPC距离目标点足够近时，就可以认为玩家到达了目标地点
        if(Vector3.Distance(target, this.transform.position)<0.1f)
        {
            //到达目的地，重新选择目标位置
            StartCoroutine(SetNowTarget()); 
        }
    }
    //重新选择目标位置的 协程
    IEnumerator SetNowTarget()
    {
        //进行暂停移动
        isPause = true;
        yield return new WaitForSeconds(pauseTime);
        //等待结束，进行选择新的目标
        target = patrolArrs[targetIndex % patrolArrs.Length];
        targetIndex++;
        isPause = false;
    }
}
