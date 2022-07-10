using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill/Player/MultiShot", fileName = "Player_Skill_MultiShot")]
public class MultiShot : Skill
{
    public override void Execute_Skill(WeaponAnimation owner)
    {
        if (owner.isMultiShot)
        {
            return;
        }
        
        owner.isMultiShot = true;
    }
}
