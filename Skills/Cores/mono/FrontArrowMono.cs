using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontArrowMono : MonoBehaviour
{
    [HideInInspector] public WeaponAnimation owner;
    private bool _isChange;
    
    public void Execute_FrontArrow()
    {
        if (_isChange is true)
        {
            return;
        }
        
        owner.objName = "Double" + owner.objName;
        _isChange = true;
    }
}
