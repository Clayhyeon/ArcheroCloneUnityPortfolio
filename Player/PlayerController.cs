using System.Collections.Generic;
using UnityEngine;


public class PlayerController : ControllerBase
{
    #region Player Event

    public delegate void PlayerInitHandler(PlayerController owner);
    public event PlayerInitHandler OnPlayerInit = null;
    
    #endregion
    
    public WeaponConfig weaponConfig;
    private List<PlayerComponent> _playerComponents;
    
    public override void Init(StageManager sm, ConfigBase configBase)
    {
        #region EquipComponets

        _playerComponents = new List<PlayerComponent>
        {
            gameObject.AddComponent<PlayerAttackController>()
        };

        foreach (var component in _playerComponents)
        {
            component.Init(this);
        }
        
        #endregion
        
        #region Config Parsing

        Config = configBase;
        weaponConfig = (WeaponConfig) Config;
        name = weaponConfig.Name;
        
        #endregion
            
        #region Components
        
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        Sm = sm;
        
        #endregion
        
        #region Life

        dead = false;
        Hp = 100f;
        
        #endregion
        
        #region Movement

        MoveDir = Vector3.zero;
        MoveSpeed = 5.0f;
        
        GameManager.Input.MovementEventHandler -= OnMovement;
        GameManager.Input.MovementEventHandler += OnMovement;
        
        #endregion

        #region Init Event

        OnPlayerInit?.Invoke(this);
        
        #endregion
        
    }

    private void OnMovement(Types.State changeState, Vector3 dir)
    {
        if (Hp <= 0f)
        {
            Rb.velocity = Vector3.zero;
            State = Types.State.Dead;
            
            return;
        }
        
        if (Sm.isStart is false || Sm.isNext)
        {
            Rb.velocity = Vector3.zero;
            State = Types.State.Idle;
            
            return;
        }
        
        MoveDir = dir * MoveSpeed;
        State = changeState;
    }
    
    protected override void UpdateIdle()
    {
        if (Sm.isStart is false || Sm.isNext ||  Sm.IsTarget is false)
        {
            Rb.velocity = Vector3.zero;
            State = Types.State.Idle;
            return;
        }

        if (Sm.IsTarget)
        {
            State = Types.State.Attack;
        }
    }

    protected override void UpdateMovement()
    {
        Rb.velocity = new Vector3(MoveDir.x, Rb.velocity.y, MoveDir.y);
        Rb.rotation = Quaternion.LookRotation(new Vector3(MoveDir.x, 0, MoveDir.y));

        base.UpdateMovement();
    }

    protected override void UpdateAttack()
    {
        var target = Sm.GetTarget(transform);
        
        if (target is null)
        {
            Rb.velocity = Vector3.zero;
            State = Types.State.Idle;
            return;
        }
        
        Rb.velocity = Vector3.zero;
        transform.LookAt(target);
        base.UpdateAttack();
    }

    protected override void UpdateDead()
    {
        GameManager.Input.MovementEventHandler -= OnMovement;
        Rb.velocity = Vector3.zero;
        base.UpdateDead();
    }
}
