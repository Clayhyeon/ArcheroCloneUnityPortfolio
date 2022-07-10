using UnityEngine;


public abstract class Skill : ScriptableObject
{
    public Types.Skill type;
    public abstract void Execute_Skill(WeaponAnimation owner);
}
