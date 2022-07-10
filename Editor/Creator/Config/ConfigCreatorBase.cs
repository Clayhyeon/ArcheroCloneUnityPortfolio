using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ConfigCreatorBase : EditorWindow
{
    protected ConfigBase[] Configs;

    private SerializedObject _serializedObject;
    private SerializedProperty _serializedProperty;
    private string _selectedPropertyName;
    private string _selectedProperty;

    public ConfigBase newConfig;
    public Data asset;
    
    private int _nextId;


    
    protected void CreateConfigOnGUI(string configPath, string assetPath)
    {
        _serializedObject = new SerializedObject(newConfig);
        _serializedProperty = _serializedObject.GetIterator();
        _serializedProperty.NextVisible(true);
            
        DrawProperties(_serializedProperty);
            
        
        if (GUILayout.Button("저장"))
        {
            NextId();
            if (IdOverlapCheck())
            {
                EditorUtility.DisplayDialog("중복되는 ID가 존재 합니다.", $"새로운 ID를 입력 해주세요. 가능한 아이디 넘버 : {_nextId}", "확인");
            }
            else if (newConfig.Id > _nextId)
            {
                EditorUtility.DisplayDialog("ID 순서가 맞지 않습니다.", $"오름차순으로 작성 해주세요. 가능한 아이디 넘버 : {_nextId}", "확인");

            }
            else if (newConfig.Id < 0)
            {
                EditorUtility.DisplayDialog("ID가 옳지 않습니다.", $"ID Number는 음수는 사용이 불가능 합니다. 가능한 아이디 넘버 : {_nextId}", "확인");
            }
            else if (NameOverlapCheck())
            {
                EditorUtility.DisplayDialog("중복되는 이름이 존재 합니다.", "새로운 이름을 입력 해주세요.", "확인");

            }
            else
            {
                AssetDatabase.CreateAsset(newConfig, configPath + newConfig.Name + ".asset");
                asset.confing = newConfig;
                AssetDatabase.CreateAsset(asset, assetPath + asset.confing.Name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Close();
            }
        }
        Apply();
    }
    
    private void NextId()
    {
        if (Configs.Length.Equals(0))
        {
            _nextId = 0;
        }
        
        var ids = new int[Configs.Length];
        for (var i = 0; i < ids.Length; i++)
        {
            ids[i] = Configs[i].Id;
        }

        _nextId = ids.Max() + 1;
    }
    private bool IdOverlapCheck()
    {
        return Configs.Any(configBase => configBase.Id == newConfig.Id);
    }

    private bool NameOverlapCheck()
    {
        return Configs.Any(configBase => configBase.Name == newConfig.Name);
    }
        
    protected static ConfigBase[] FindConfig<T>() where T : ConfigBase
    {
        var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

        var configs = new ConfigBase[guids.Length];
            
        for (var i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            configs[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return configs;
    }
        
    private void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

        }
    }
        
    private void Apply()
    {
        _serializedObject.ApplyModifiedProperties();
    }
}
