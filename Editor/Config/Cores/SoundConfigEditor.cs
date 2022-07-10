using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundConfigEditor : ConfigEditorBase<SoundConfigEditor>
{
    [MenuItem("Tools/Open Sound Config Editor")]
    private static void Open()
    {
        ShowConfig("Sound Config Editor", "d_CustomTool");
    }
    private void OnGUI()
    {
        Configs = FindConfig<SoundConfig>();
        SerializedObject = new SerializedObject(Configs[0]);
        DrawButton();
        DrawTitle("사운드 오브젝트 편집기");
        DrawConfig<SoundConfig>();
    }
    
    protected override void CreateNewConfig()
    {
        var newWeaponConfig = CreateInstance<SoundConfig>();
        var newWeaponConfigWindow = GetWindow<SoundConfigCreator>();
        newWeaponConfigWindow.titleContent = new GUIContent("새로운 사운드 오브젝트 만들기");
        newWeaponConfigWindow.newConfig = newWeaponConfig;
        newWeaponConfigWindow.asset = CreateInstance<Sound>();
        newWeaponConfigWindow.ShowUtility();
    }
}
