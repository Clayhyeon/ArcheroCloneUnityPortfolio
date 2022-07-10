using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SkillTable))]
public class SkillTableEditor : Editor
{
    [MenuItem("Assets/Table/Open SKill Table")]
    public static void OpenInspector()
    {
        Selection.activeObject = SkillTable.Instance;
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
