using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill/Player/FrontArrow", fileName = "Player_Skill_FrontArrow")]
public class FrontArrow : Skill
{
    public override void Execute_Skill(WeaponAnimation owner)
    {
        if (owner.GetComponent<FrontArrowMono>() is not null)
        {
            return;
        }
        
        owner.AddComponent<FrontArrowMono>();
        owner.GetComponent<FrontArrowMono>().owner = owner;
        owner.GetComponent<FrontArrowMono>().Execute_FrontArrow();
    }
}
