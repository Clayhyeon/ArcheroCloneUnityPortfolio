using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(UIDatabase))]
public class UIDatabaseEditor : Editor
{
    [MenuItem("Assets/Database/Open UI Database")]
    public static void OpenInspector()
    {
        Selection.activeObject = UIDatabase.Instance;
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
