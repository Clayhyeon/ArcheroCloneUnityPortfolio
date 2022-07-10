using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacterConfigEditor : ConfigEditorBase<PlayerCharacterConfigEditor>
{
    [MenuItem("Tools/Open Player Character Config Editor")]
    private static void Open()
    {
        ShowConfig("Player Character Config Editor", "d_CustomTool");
    }
    private void OnGUI()
    {
        Configs = FindConfig<PlayerCharacterConfig>();
        SerializedObject = new SerializedObject(Configs[0]);
        DrawButton();
        DrawTitle("플레이어 캐릭터 편집기");
        DrawConfig<PlayerCharacterConfig>();
    }
    
    protected override void CreateNewConfig()
    {
        var newWeaponConfig = CreateInstance<PlayerCharacterConfig>();
        var newWeaponConfigWindow = GetWindow<PlayerCharacterConfigCreator>();
        newWeaponConfigWindow.titleContent = new GUIContent("새로운 플레이어 캐릭터 만들기");
        newWeaponConfigWindow.newConfig = newWeaponConfig;
        newWeaponConfigWindow.asset = CreateInstance<PlayerCharacter>();
        newWeaponConfigWindow.ShowUtility();
    }
}
