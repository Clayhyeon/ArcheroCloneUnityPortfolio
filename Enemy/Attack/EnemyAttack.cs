using UnityEngine;


public abstract class EnemyAttack : ScriptableObject
{
    public abstract void Attack(Transform owner);
}
