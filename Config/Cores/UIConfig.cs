using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Config/UI", fileName = "Config_UI_")]
public class UIConfig : ConfigBase
{
    [field: Space(10f)] 
    [field: SerializeField] public Types.UIType type;
}
