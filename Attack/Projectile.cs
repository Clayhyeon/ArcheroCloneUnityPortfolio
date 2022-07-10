using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Owner
{
    Player,
    Enemy
}

public class Projectile : MonoBehaviour
{
    #region Events

    public delegate void SkillExecuteHandler(Projectile owner);

    public event SkillExecuteHandler OnSkillExecute = null;

    #endregion

    #region Info
    
    [SerializeField] private string projectileName;
    
    public Vector3 resetPos;
    public Owner owner;

    public float power;
    public float speed;


    #endregion

    #region Components

    public StageManager stageManager;


    #endregion

    #region Skill

    public GameObject obj;
    public bool isSkill;

    #endregion

    #region State

    private bool _isStart;

    #endregion
    
    private void Start()
    {
        stageManager = GameObject.Find(nameof(StageManager)).GetComponent<StageManager>();
    }

    private void Update()
    {
        if (_isStart)
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
    }

    public void Init(float power, float speed)
    {
        this.power = power;
        this.speed = speed;
        _isStart = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isStart is false)
        {
            return;
        }
        
        if (owner == Owner.Player && other.gameObject.CompareTag("Player") || owner == Owner.Enemy && other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        obj = other.gameObject.CompareTag("Enemy") ? other.gameObject : null;
        
        OnSkillExecute?.Invoke(this);
        OnHit();
        
        if (other.GetComponent<ControllerBase>())
        {
            other.gameObject.GetComponent<ControllerBase>().UpdateDamaged(power);
        }
    }

    private void OnHit()
    {
        GameManager.Pooling.GetObject(transform.position, Quaternion.identity,  projectileName + "Hit");
        GameManager.Sound.Play(transform, projectileName + "HitSound");

        if (isSkill is false)
        {
            GameManager.Pooling.ReturnObject(transform, projectileName);
        }
    }

    public void Reset_Projectile()
    {
        gameObject.transform.position = resetPos;
    }
}
