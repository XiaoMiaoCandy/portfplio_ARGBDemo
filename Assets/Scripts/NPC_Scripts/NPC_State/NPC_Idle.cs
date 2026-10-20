using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Idle : MonoBehaviour
{
    //动画播放器和NPC刚体
    private Rigidbody2D rb;         //驱动NPC移动的脚本
    private Animator animator;      //驱动动画播放的组件
    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //NPC的动画由NPC_Sprite驱动
        animator = GetComponentInChildren<Animator>();
        //NPC暂停移动，播放待机动画
        animator.Play("Idle");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void OnDisable()
    {
        rb.isKinematic = false;
    }
}
