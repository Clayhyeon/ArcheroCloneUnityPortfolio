using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill/Player/Ricochet", fileName = "Player_Skill_Ricochet")]
public class Ricochet : Skill
{
    public override void Execute_Skill(WeaponAnimation owner)
    {
        if (owner.projectile.GetComponent<RicochetMono>() is null)
        {
            owner.projectile.AddComponent<RicochetMono>();
            owner.projectile.GetComponent<Projectile>().OnSkillExecute += owner.projectile.GetComponent<RicochetMono>().Execute_Ricochet;
            owner.projectile.GetComponent<Projectile>().isSkill = true;
        }
    }
}
