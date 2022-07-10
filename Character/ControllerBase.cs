using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public abstract class ControllerBase : MonoBehaviour
{
    protected ConfigBase Config;
    
    #region Events

    public delegate void StateChangeHandler(ControllerBase owner);
    
    public event StateChangeHandler OnStateIdle = null;
    public event StateChangeHandler OnStateMovement = null;
    public event StateChangeHandler OnStateAttack = null;
    public event StateChangeHandler OnStateDamaged = null;
    public event StateChangeHandler OnStateDead = null;
    
    #endregion

    #region State

    private Types.State _state;

    public Types.State State
    {
        get => _state;
        set
        {
            if (name is null || Anim is null)
            {
                return;
            }
            
            _state = value;
            Anim.CrossFade(name + "_" + State.ToString(), 0.0001f);
        }
    }

    #endregion

    #region Movement

    protected Vector3 MoveDir;

    private float _moveSpeed;

    protected float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (value < 0)
            {
                _moveSpeed = 0;
            }
            else
            {
                _moveSpeed = value;
            }
        }
    }

    #endregion

    #region Components
    
    protected Rigidbody Rb;
    protected Animator Anim;
    protected StageManager Sm;

    #endregion
    
    #region Life

    protected int DamagedPower;
    public bool dead;
    public float damage;
    private float _hp;
    public float Hp
    {
        get => _hp;
        protected set
        {
            _hp = value > 100f ? 100f : value;

            if (Hp > 0f)
            {
                return;
            }
            
            State = Types.State.Dead;
        }
    }

    #endregion

    #region Strings

    public new string name;    

    #endregion

    private void Awake()
    {
        State = Types.State.Idle;
    }

    private void Update()
    {
        if (dead is false)
        {
            (State switch
            {
                Types.State.Idle => (Action) UpdateIdle,
                Types.State.Movement => UpdateMovement,
                Types.State.Attack => UpdateAttack,
                Types.State.Dead => UpdateDead,
                _ => throw new ArgumentOutOfRangeException()
            })();
        }
    }

    public abstract void Init(StageManager sm, ConfigBase configBase);
 

    protected virtual void UpdateIdle()
    {
        OnStateIdle?.Invoke(this);
    }

    protected virtual void UpdateMovement()
    {
        OnStateMovement?.Invoke(this);
    }

    protected virtual void UpdateAttack()
    {
        OnStateAttack?.Invoke(this);
    }

    public void UpdateDamaged(float power)
    {
        var randomDamage = new Random();
        damage = randomDamage.Next((int)power - 3, (int)power + 3);
        
        if (damage < 1)
        {
            damage = 1;
        }
        
        Hp -= damage;
        OnStateDamaged?.Invoke(this);
    }

    protected virtual void UpdateDead()
    {
        dead = true;
        OnStateDead?.Invoke(this);
        
        OnStateIdle = null;
        OnStateMovement = null;
        OnStateAttack = null;
        OnStateDamaged = null;
        OnStateDead = null;
    }
}