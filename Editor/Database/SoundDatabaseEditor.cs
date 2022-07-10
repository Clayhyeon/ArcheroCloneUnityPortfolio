using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundDatabase))]
public class SoundDatabaseEditor : Editor
{
    [MenuItem("Assets/Database/Open Sound Database")]
    public static void OpenInspector()
    {
        Selection.activeObject = SoundDatabase.Instance;
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
