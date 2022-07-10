using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Config/Weapon", fileName = "Config_Weapon_")]
public class WeaponConfig : ConfigBase
{
    #region Stat

    [field: Space(10f)] 
    [field: SerializeField] public Types.WeaponType type;

    [field: Space(10f)] 
    [field: SerializeField] public WeaponFunction function;
    [field: SerializeField] public float attackPower;
    [field: SerializeField] public float attackSpeed;
    #endregion

    #region Method

    public GameObject Equip(CharacterParts parts)
    {
        return function.Equip(this, model, parts);
    }

    #endregion
}
