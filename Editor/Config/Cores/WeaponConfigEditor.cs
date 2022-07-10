using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponConfigEditor : ConfigEditorBase<WeaponConfigEditor>
{
    [MenuItem("Tools/Open Weapon Config Editor")]
    private static void Open()
    {
        ShowConfig("Weapon Config Editor", "d_CustomTool");
    }
    
    private void OnGUI()
    {
        Configs = FindConfig<WeaponConfig>();
        SerializedObject = new SerializedObject(Configs[0]);
        DrawButton();
        DrawTitle("무기 편집기");
        DrawConfig<WeaponConfig>();
    }
    
    protected override void CreateNewConfig()
    {
        var newWeaponConfig = CreateInstance<WeaponConfig>();
        var newWeaponConfigWindow = GetWindow<WeaponConfigCreator>();
        newWeaponConfigWindow.titleContent = new GUIContent("새로운 무기 만들기");
        newWeaponConfigWindow.newConfig = newWeaponConfig;
        newWeaponConfigWindow.asset = CreateInstance<Weapon>();
        newWeaponConfigWindow.ShowUtility();
    }
}
