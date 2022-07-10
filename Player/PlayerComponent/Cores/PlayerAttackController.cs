using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerComponent
{
    private CharacterParts _parts;
    private PlayerController _controller;
    private GameObject _weapon;
    private Transform _spawn;
    public override void Init(PlayerController owner)
    {
        _controller = owner;
        _controller.OnPlayerInit += OnEquipWeapon;
    }
    
    private void OnEquipWeapon(PlayerController owner)
    {
        _parts = GetComponent<CharacterParts>();
        _weapon = owner.weaponConfig.Equip(_parts);
        _spawn = _weapon.transform.Find("Spawn");
        owner.OnStateAttack += OnAttack;
    }

    private void OnAttack(ControllerBase owner)
    {
        _controller = (PlayerController) owner;
        _controller.weaponConfig.function.Attack(owner.transform, _spawn);
    }
}
