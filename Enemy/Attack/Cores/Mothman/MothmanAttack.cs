using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/Attack/Mothman", fileName = "EnemyAttack_Mothman")]
public class MothmanAttack : EnemyAttack
{
    public override void Attack(Transform owner)
    {
        if (owner.GetComponent<MothmanAttackAnimation>() is null)
        {
            owner.AddComponent<MothmanAttackAnimation>();
        }
    }

    public MothmanAttack Clone()
    {
        return this;
    }
}
