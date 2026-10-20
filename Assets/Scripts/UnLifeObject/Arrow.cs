using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("基础设置")]
    public float moveSpeed = 20f;   // 子弹飞行速度
    public float destroyTime = 2f;  // 多久后自动销毁（防止漏飞）
    public Transform owner;
    private Rigidbody2D arrowRb;       

    void Awake()
    {
        // 获取刚体组件
        arrowRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    private void Update()
    {
        arrowRb.velocity = transform.right * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D targetCollider)
    {
        if(owner.CompareTag("Player")&&targetCollider.CompareTag("Monster"))
        {
            LivingEntity target = targetCollider.gameObject.GetComponent<LivingEntity>();
            Player.Instance.AttackOther(target);
        }
    }
}
