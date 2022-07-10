using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIConfigEditor : ConfigEditorBase<UIConfigEditor>
{
    [MenuItem("Tools/Open UI Config Editor")]
    private static void Open()
    {
        ShowConfig("UI Config Editor", "d_CustomTool");
    }
    private void OnGUI()
    {
        Configs = FindConfig<UIConfig>();
        SerializedObject = new SerializedObject(Configs[0]);
        DrawButton();
        DrawTitle("UI 오브젝트 편집기");
        DrawConfig<UIConfig>();
    }
    
    protected override void CreateNewConfig()
    {
        var newWeaponConfig = CreateInstance<UIConfig>();
        var newWeaponConfigWindow = GetWindow<UIConfigCreator>();
        newWeaponConfigWindow.titleContent = new GUIContent("새로운 UI 오브젝트 만들기");
        newWeaponConfigWindow.newConfig = newWeaponConfig;
        newWeaponConfigWindow.asset = CreateInstance<UI>();
        newWeaponConfigWindow.ShowUtility();
    }
}
