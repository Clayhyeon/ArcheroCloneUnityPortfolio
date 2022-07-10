using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : ControllerBase
{
    private NavMeshAgent _agent;
    public EnemyConfig enemyConfig;

    public bool isAttack;

    [SerializeField] private float radius;
    private bool _isWander;

    [SerializeField] private bool isTestStop;
    public override void Init(StageManager sm, ConfigBase configBase)
    {
        #region Config Parsing

        Config = configBase;
        enemyConfig = (EnemyConfig) Config;
        
        if (this.Config != null)
        {
            name = Config.Name;
            
            #region Life

            dead = false;
            Hp = enemyConfig.hp;
            
            #endregion
        }
        
        #endregion
        
        #region Movement

        MoveDir = Vector3.forward;
        MoveSpeed = 2.0f;

        #endregion

        #region Components

        Sm = sm;
        Anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        Rb = GetComponent<Rigidbody>();

        #endregion

        #region Attack

        isAttack = false;

        #endregion

    }
    
    protected override void UpdateIdle()
    {
        if (Sm.isStart is false || !Sm.IsPlayer || _isWander || isTestStop)
        {
            State = Types.State.Idle;
            base.UpdateIdle();
            return;
        }
        
        State = Types.State.Movement;
    }

    protected override void UpdateMovement()
    {
        State = Types.State.Movement;

        if (_isWander)
        {
            if (AttackCheck() || ChaseCheck())
            {
                StopCoroutine(WanderDelay());
                _isWander = false;
            }
            else
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        StartCoroutine(WanderDelay());
                        State = Types.State.Idle;
                    }
                }
            }

            return;
        }
        
        if (ChaseCheck())
        {
            var target = Sm.GetPlayer();
            _agent.SetDestination(target.position);

            var rot = _agent.steeringTarget - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rot), Time.deltaTime * 5f);

        }
        else
        {
            _isWander = true;
            _agent.SetDestination(Wander());
        }
        
        State = AttackCheck() ? Types.State.Attack : Types.State.Movement;
    }

    protected override void UpdateAttack()
    {
        switch (isAttack)
        {
            case true:
                return;
            case false:
                isAttack = true;
                break;
        }

        GetComponent<Collider>().isTrigger = true;
        base.UpdateAttack();
        
        var target = Sm.GetPlayer();
        transform.LookAt(target);
        _agent.SetDestination(transform.position);
    }

    protected override void UpdateDead()
    {
        _agent.SetDestination(transform.position);

        GetComponent<Collider>().enabled = false;
        _agent.enabled = false;
        base.UpdateDead();
    }

    private Vector3 Wander()
    {
        var wanderPos = UnityEngine.Random.onUnitSphere;
        wanderPos.y = 0f;

        var r = UnityEngine.Random.Range(0.0f, radius);
        return (wanderPos * r) + transform.position;
    }

    private IEnumerator WanderDelay()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1,4));
        
        _isWander = false;
        
        yield return null;
    }
    
    private bool ChaseCheck()
    {
        return enemyConfig.distance.chase > Vector3.Distance(transform.position, Sm.GetPlayer().position);
    }
    private bool AttackCheck()
    {
        return enemyConfig.distance.attack > Vector3.Distance(transform.position, Sm.GetPlayer().position);
    }
    
}