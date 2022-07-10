using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Config/Sound", fileName = "Config_Sound_")]
public class SoundConfig : ConfigBase
{
    [field: Space(10f)] 
    [field: SerializeField] public Types.SoundType type;
}
