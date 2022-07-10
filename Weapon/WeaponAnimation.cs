using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private Transform _spawn;
    private WeaponConfig _config;
    public string objName;

    public bool isMultiShot;
    public GameObject projectile;

    public void Init(WeaponConfig config, Transform spawn, string objName)
    {
        _spawn = spawn;
        _config = config;
        this.objName = objName;
    }

    public void AttackAnimation()
    {
        Fire();
        
        if (isMultiShot)
        {
            Invoke(nameof(Fire), 0.25f);
        }
    }

    private void Fire()
    {
        projectile = GameManager.Pooling.GetObject(_spawn.position, _spawn.rotation, objName);
        
        if (PlayerData.Instance.Skills.Count != 0)
        {
            foreach (var skill in PlayerData.Instance.Skills)
            {
                skill.Execute_Skill(this);
            }
        }
        
        projectile.GetComponent<Projectile>().Init(_config.attackPower, _config.attackSpeed);
        
    }
}