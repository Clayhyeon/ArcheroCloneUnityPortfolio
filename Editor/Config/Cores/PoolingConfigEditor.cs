using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolingConfigEditor : ConfigEditorBase<PoolingConfigEditor>
{
    [MenuItem("Tools/Open Pooling Config Editor")]
    private static void Open()
    {
        ShowConfig("Pooling Config Editor", "d_CustomTool");
    }
    private void OnGUI()
    {
        Configs = FindConfig<PoolingConfig>();
        SerializedObject = new SerializedObject(Configs[0]);
        DrawButton();
        DrawTitle("풀링 오브젝트 편집기");
        DrawConfig<PoolingConfig>();
    }
    
    protected override void CreateNewConfig()
    {
        var newWeaponConfig = CreateInstance<PoolingConfig>();
        var newWeaponConfigWindow = GetWindow<PoolingConfigCreator>();
        newWeaponConfigWindow.titleContent = new GUIContent("새로운 풀링 오브젝트 만들기");
        newWeaponConfigWindow.newConfig = newWeaponConfig;
        newWeaponConfigWindow.asset = CreateInstance<Pooling>();
        newWeaponConfigWindow.ShowUtility();
    }
    
}
