using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCharacterDatabase))]
public class PlayerCharacterDatabaseEditor : Editor
{
    [MenuItem("Assets/Database/Open Player Character Database")]
    public static void OpenInspector()
    {
        Selection.activeObject = PlayerCharacterDatabase.Instance;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!GUI.changed)
        {
            return;
        }
            
        EditorUtility.SetDirty(target); // 변경사항이 강제로 동기화 되게 함.
        AssetDatabase.SaveAssets();
    }
}
