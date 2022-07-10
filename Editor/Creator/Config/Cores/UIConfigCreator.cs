using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConfigCreator : ConfigCreatorBase
{
    private const string ConfigPath = "Assets/Resources/ScriptableObjects/Data/Configs/UI/Config_UI_";
    private const string AssetPath = "Assets/Resources/ScriptableObjects/Data/UI/UI_";
    private void OnGUI()
    {
        CreateConfigOnGUI(ConfigPath, AssetPath);
        Configs = FindConfig<UIConfig>();
    }
}
