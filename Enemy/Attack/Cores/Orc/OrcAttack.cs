using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/Attack/Orc", fileName = "EnemyAttack_Orc")]
public class OrcAttack : EnemyAttack
{
    public override void Attack(Transform owner)
    {
        if (owner.GetComponent<OrcAttackAnimation>() is null)
        {
            owner.AddComponent<OrcAttackAnimation>();
        }
    }
}