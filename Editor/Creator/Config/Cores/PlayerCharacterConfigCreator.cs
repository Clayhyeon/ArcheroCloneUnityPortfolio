using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterConfigCreator : ConfigCreatorBase
{
    private const string ConfigPath = "Assets/Resources/ScriptableObjects/Data/Configs/PlayerCharacters/Config_PlayerCharacter_";
    private const string AssetPath = "Assets/Resources/ScriptableObjects/Data/PlayerCharacter/PlayerCharacter_";
    
    private void OnGUI()
    {
        CreateConfigOnGUI(ConfigPath, AssetPath);
        Configs = FindConfig<PlayerCharacterConfig>();
    }
}
