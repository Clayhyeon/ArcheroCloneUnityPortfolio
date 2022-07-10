using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Distance
{
    public float chase;
    public float attack;
}

[CreateAssetMenu(menuName = "Data/Config/Enemy", fileName = "Config_Enemy_")]
public class EnemyConfig : ConfigBase
{
    #region Config

    [field: Space(10f)]
    [field: SerializeField] public Types.EnemyCategory Category { get; set; }

    [field: SerializeField] public Types.EnemyAttackType AttackType { get; set; }

    #endregion

    #region Stats

    [field: Space(10f)] 
    [field: SerializeField] public float hp;

    [field: Space(10f)] 
    [field: SerializeField] public EnemyAttack attack;
    [field: SerializeField] public float attackPower;
    [field: SerializeField] public float attackSpeed;

    [field: Space(10f)] 
    [field: SerializeField] public int money;
    [field: SerializeField] public int exp;
    
    [field: Space(10f)] 
    [field: SerializeField] public Distance distance;
    
    #endregion

}