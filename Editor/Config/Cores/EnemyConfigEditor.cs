using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyConfigEditor : ConfigEditorBase<EnemyConfigEditor>
{
    [MenuItem("Tools/Open Enemy Config Editor")]
    private static void Open()
    {
        ShowConfig("Enemy Config Editor", "d_CustomTool");
    }
    
    private void OnGUI()
    {
        Configs = FindConfig<EnemyConfig>();
        SerializedObject = new SerializedObject(Configs[0]);
        DrawButton();
        DrawTitle("몬스터 편집기");
        DrawConfig<EnemyConfig>();
    }

    protected override void CreateNewConfig()
    {
        var newEnemyConfig = CreateInstance<EnemyConfig>();
        var newEnemyConfigWindow = GetWindow<EnemyConfigCreator>();
        newEnemyConfigWindow.titleContent = new GUIContent("새로운 몬스터 만들기");
        newEnemyConfigWindow.newConfig = newEnemyConfig;
        newEnemyConfigWindow.asset = CreateInstance<Enemy>();
        newEnemyConfigWindow.ShowUtility();
    }
}