using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Config/Pooling", fileName = "Config_Pooling_")]
public class PoolingConfig : ConfigBase
{
    [field: Space(10f)] 
    [field: SerializeField] public Types.PoolingType type;
}
