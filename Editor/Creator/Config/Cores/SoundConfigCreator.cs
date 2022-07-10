using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundConfigCreator : ConfigCreatorBase
{
    private const string ConfigPath = "Assets/Resources/ScriptableObjects/Data/Configs/Sounds/Config_Sound_";
    private const string AssetPath = "Assets/Resources/ScriptableObjects/Data/Sound/Sound_";
    private void OnGUI()
    {
        CreateConfigOnGUI(ConfigPath, AssetPath);
        Configs = FindConfig<SoundConfig>();
    }
}
