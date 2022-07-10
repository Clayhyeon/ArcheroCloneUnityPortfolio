using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnimation : MonoBehaviour
{
    protected EnemyController Controller;
    protected StageManager Sm;
    private void Start()
    {
        Controller = GetComponent<EnemyController>();
        Sm = GameObject.Find(nameof(StageManager)).GetComponent<StageManager>();
    }

    public virtual void AttackStart()
    {
        
    }
    
    public void AttackEnd()
    {
        Controller.State = Types.State.Idle;
        Controller.isAttack = false;
        Controller.GetComponent<Collider>().isTrigger = false;
    }
}
