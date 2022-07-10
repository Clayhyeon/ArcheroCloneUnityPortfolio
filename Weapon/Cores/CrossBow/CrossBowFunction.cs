using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon/Function/CrossBow", fileName = "WeaponFunction_CrossBow")]
public class CrossBowFunction : WeaponFunction
{
    private WeaponConfig _config;
    
    public override GameObject Equip(WeaponConfig config, GameObject model, CharacterParts parts)
    {
        _config = config;
        
        var obj = Instantiate(model, parts.leftHand, true);
        obj.transform.localPosition = model.transform.localPosition;
        obj.transform.localRotation = model.transform.localRotation;
        
        return obj;
    }

    public override void Attack(Transform owner, Transform spawn)
    {
        if (owner.GetComponent<WeaponAnimation>() is not null)
        {
            return;
        }
        
        var animation = owner.AddComponent<WeaponAnimation>();
        animation.Init(_config, spawn, "Arrow");
    }
}
