using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDatabase))]
public class EnemyDatabaseEditor : Editor
{
    [MenuItem("Assets/Database/Open Enemy Database")]
    public static void OpenInspector()
    {
        Selection.activeObject = EnemyDatabase.Instance;
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
