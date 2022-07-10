using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfigCreator : ConfigCreatorBase
{
    private const string ConfigPath = "Assets/Resources/ScriptableObjects/Data/Configs/Weapons/Config_Weapon_";
    private const string AssetPath = "Assets/Resources/ScriptableObjects/Data/Weapon/Weapon_";
    private void OnGUI()
    {
        CreateConfigOnGUI(ConfigPath, AssetPath);
        Configs = FindConfig<WeaponConfig>();
    }
}
