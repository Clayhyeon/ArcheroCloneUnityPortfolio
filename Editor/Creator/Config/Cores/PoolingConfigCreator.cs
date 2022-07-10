using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingConfigCreator : ConfigCreatorBase
{
    private const string ConfigPath = "Assets/Resources/ScriptableObjects/Data/Configs/Poolings/Config_Pooling_";
    private const string AssetPath = "Assets/Resources/ScriptableObjects/Data/Pooling/Pooling_";
    private void OnGUI()
    {
        CreateConfigOnGUI(ConfigPath, AssetPath);
        Configs = FindConfig<PoolingConfig>();
    }
}
