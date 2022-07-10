using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private EnemyController _controller;
    
    private void Start()
    {
        _controller = GetComponent<EnemyController>();
        _controller.OnStateAttack += OnAttack;
    }

    private void OnAttack(ControllerBase owner)
    {
       _controller.enemyConfig.attack.Attack(owner.transform);
    }
}
