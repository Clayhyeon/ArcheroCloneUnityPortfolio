using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfigCreator : ConfigCreatorBase
{
    private const string ConfigPath = "Assets/Resources/ScriptableObjects/Data/Configs/Enemies/Config_Enemy_";
    private const string AssetPath = "Assets/Resources/ScriptableObjects/Data/Enemy/Enemy_";
    private void OnGUI()
    {
        CreateConfigOnGUI(ConfigPath, AssetPath);
        Configs = FindConfig<EnemyConfig>();
    }
}
