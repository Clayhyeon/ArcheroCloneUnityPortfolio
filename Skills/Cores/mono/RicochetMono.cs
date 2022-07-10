using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetMono : MonoBehaviour
{
    private const int MaxHit = 3;
    [SerializeField] private int currentHit = 0;
    
    public void Execute_Ricochet(Projectile owner)
    {
        if (owner.obj is null)
        {
            if (owner.isSkill is true)
            {
                owner.isSkill = false;
            }
            
            return;
        }
        
        
        if (currentHit < MaxHit)
        {
            currentHit++;
        }

        
        if (owner.stageManager.enemies.Count > 1)
        {
            var newTarget = owner.stageManager.GetTarget(owner.obj.transform);
            var newPos = newTarget.position;

            newPos = new Vector3(newPos.x, transform.position.y,
                newPos.z);
            
            owner.transform.LookAt(newPos);
        
            owner.power = owner.power - (owner.power * 0.3f);
        }


        if (currentHit != MaxHit)
        {
            return;
        }
        
        if (owner.isSkill is true)
        {
            owner.isSkill = false;
        }
    }
}
