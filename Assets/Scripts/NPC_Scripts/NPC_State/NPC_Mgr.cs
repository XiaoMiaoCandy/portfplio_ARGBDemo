using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 该脚本是NPC行为状态的管理器，用来统一调控某一NPC当前的行为
/// 挂载在NPC上
/// </summary>
public class NPC_Mgr : MonoBehaviour
{
    //NPC的行为状态
    public enum E_NPCState
    {
        Default,    //默认状态
        Idle,       //空闲待机
        Patrol,     //巡逻
        Wander,     //游荡
        Dialogue    //交流
    }

    //NPC的行为状态
    public E_NPCState nowState;     //NPC的当前行为状态
    private E_NPCState defaultState = E_NPCState.Wander; //NPC的默认状态

    //NPC个状态的对应脚本 => 管理器通过控制各脚本的激活失活来进行状态的调度
    public NPC_Patrol patrol;       //巡逻
    public NPC_Wander wander;       //游荡
    public NPC_Dialogue dialogue;   //交流
    public NPC_Idle idle;           //待机

    private void Start()
    {
        defaultState = nowState;
        
    }
    //进行行为状态切换方法 => 通过该方法可以将当前行为切换到state对应状态
    public void SwitchState(E_NPCState state)
    {
        //当前状态
        nowState = state;
        //如果当前状态为Patrol则该脚本 激活反之失活
        patrol.enabled = nowState == E_NPCState.Patrol;
        wander.enabled = nowState == E_NPCState.Wander;
        dialogue.enabled = nowState == E_NPCState.Dialogue;
        idle.enabled = nowState == E_NPCState.Idle;
        //if(nowState == E_NPCState.Dialogue)
        //{
        //    Player.Instance.PlayerStopMove();
        //}
        //else
        //{
        //    Player.Instance.RePlayerMove();
        //}
    }
    
    //NPC接触到玩家，切换到可交互状态
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SwitchState(E_NPCState.Dialogue);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SwitchState(defaultState);
        }
    }
}
