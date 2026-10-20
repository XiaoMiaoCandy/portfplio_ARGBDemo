using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该脚本用于实现NPC接触玩家后停止运动，并展示正确动画
/// </summary>
public class NPC_Dialogue : MonoBehaviour
{
    //动画播放器和NPC刚体
    private Rigidbody2D rb;         //驱动NPC移动的脚本
    private Animator animator;      //驱动动画播放的组件
    public Animator iconAnimator;   //任务图标的动画管理器
    //这里需要NPC本身的动画管理器是因为，玩家在进入交流状态时，仍然需要播放待机动画

    //当前NPC的交谈数据
    public Specker_TestSo nowSpeckInfo;
    public List<Specker_TestSo> allSpeckInfos = new();  //改NPC所有的可能对话数据

    //交谈的UI面板
    public GameObject dialoguePanel;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    //该脚本被激活时调用,表示玩家接触到NPC
    private void OnEnable()
    {
        //NPC的禁止移动
        rb.velocity = Vector2.zero;
        //播放待机动画，并显示可以交流的图标
        animator.Play("Idle");
        iconAnimator.Play("ShowIcon");
        //这个时候，NPC不希望被其他物体挤压
        rb.isKinematic = true;
    }
    //该脚本被失活时调用
    private void OnDisable()
    {
        //玩家离开NPC的可交互范围，隐藏可交互图标
        iconAnimator.Play("HideIcon");
    }

    //在进行条件对话时，通过观察是否满足条件，来是实现说不同的话,设置新对话数据
    private void CheckForNewDialogue()
    {
        for (int i = 0; i < allSpeckInfos.Count; i++)
        {
            //取出单各对话数据

            Specker_TestSo info = allSpeckInfos[i];
            if (info != null && info.IsConditonMet())
            {
                //如果条件被满足，则将这个对话输出，并将当前对话切换为该队哈
                nowSpeckInfo = info;
                if(nowSpeckInfo.removeAfterPlay)
                {
                    allSpeckInfos.RemoveAt(i);
                }
                if (nowSpeckInfo.testSoList.Count > 0 && nowSpeckInfo.testSoList != null)
                {
                    foreach(var toRemove in nowSpeckInfo.testSoList)
                    {
                        allSpeckInfos.Remove(toRemove);
                    }
                }
                
               
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(DialogueMgr.Instance == null)
            {
                dialoguePanel.SetActive(true);
                Player.Instance.PlayerStopMove();
            }
            if (DialogueMgr.Instance.isSpecking)
            {
                Player.Instance.PlayerStopMove();
                DialogueMgr.Instance.StayDialogue();
            }
            else
            {
                //当前没有正在说话，这样就开始一个新的对话
                dialoguePanel.SetActive(true);
                CheckForNewDialogue();
                DialogueMgr.Instance.StartDialogue(nowSpeckInfo); 
            }
        }
        if(!dialoguePanel.activeSelf)
        {
            Player.Instance.RePlayerMove();
        }
    }
}
