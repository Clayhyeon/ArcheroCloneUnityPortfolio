using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Table<T> : ScriptableObject where T : ScriptableObject
{
    private const string ListFileDirectory = "Assets/Resources/ScriptableObjects/Table";
    private static readonly string ListFilePath = "Assets/Resources/ScriptableObjects/Table/" + typeof(T).Name + ".asset";
    
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance is not null)
            {
                return _instance;
            }

            _instance = Resources.Load<T>("ScriptableObjects/Table/" + typeof(T).Name);

#if UNITY_EDITOR

            if (!(_instance is null))
            {
                return _instance;
            }

            if (!AssetDatabase.IsValidFolder(ListFileDirectory))
            {
                AssetDatabase.CreateFolder("Assets/Resources/ScriptableObjects", "Table");
            }

            _instance = AssetDatabase.LoadAssetAtPath<T>(ListFilePath);

            if (!(_instance is null))
            {
                return _instance;
            }

            _instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(_instance, ListFilePath);
#endif

            return _instance;
        }
    }
}
