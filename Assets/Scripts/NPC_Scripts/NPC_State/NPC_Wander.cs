using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 该脚本用于驱动NPC进行游荡
/// </summary>
public class NPC_Wander : MonoBehaviour
{
    //游荡范围大小
    public float wanderWidth = 5;   //游荡范围的宽
    public float wanderHeight = 5;  //游荡范围的高
    public float wanderSpeed = 2f;  //游荡时的速度
    public float pauseTime = 1.0f;  //NPC游荡时暂停的时长
    private bool isPause = false;   //NPC是否要暂定运动 => 只有到达指定位置或碰到其他物体时才会进行暂定
   
    //移动目标相关
    private Vector2 target;         //NPC游荡的当前目标点 => 在游荡范围内进行随机选点
    private Vector2 startPos;       //NPC印度时的初始位置

    //动画播放器和NPC刚体
    private Rigidbody2D rb;         //驱动NPC移动的脚本
    private Animator animator;      //驱动动画播放的组件
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(SetNowTarget());
        startPos = transform.position;
    }
    void Update()
    {
        if (isPause)
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
        if (direction.x < 0 && this.transform.localScale.x > 0 || direction.x > 0 && this.transform.localScale.x < 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        //玩家的移动速度 => 通过修改刚体的速度 来驱动玩家移动
        rb.velocity = direction * wanderSpeed;
        //进行距离判断 => 当NPC距离目标点足够近时，就可以认为玩家到达了目标地点
        if (Vector3.Distance(target, this.transform.position) < 0.1f)
        {
            //到达目的地，重新选择目标位置
            StartCoroutine(SetNowTarget());
        }
    }
    //NPC进行移动时，检测到新的碰撞则需要选择新的目标点
    public void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine (SetNowTarget());
    }
    //重新选择目标位置的 协程
    IEnumerator SetNowTarget()
    {
        //进行暂停移动
        isPause = true;
        yield return new WaitForSeconds(pauseTime);
        //等待结束，进行选择新的目标
        target = new Vector2(Random.Range(startPos.x - wanderWidth / 2, startPos.x + wanderWidth / 2),
            Random.Range(startPos.y - wanderHeight/2, startPos.y + wanderHeight / 2));
        isPause = false;
    }
    
}
