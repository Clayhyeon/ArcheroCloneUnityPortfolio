using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfigEditorBase<T> : EditorWindow where T : EditorWindow
{
    protected ConfigBase[] Configs;

    protected SerializedObject SerializedObject;
    private SerializedProperty _serializedProperty;

    private string _selectedPropertyName;
    private string _selectedProperty;

    protected static void ShowConfig(string title, string icon)
    {
        var window = GetWindow<T>();
        window.titleContent = new GUIContent(title, EditorGUIUtility.FindTexture(icon));
        window.Show();
    }

    protected ConfigBase[] FindConfig<TT>() where TT : ConfigBase
    {
        var guids = AssetDatabase.FindAssets($"t:{typeof(TT)}");

        var configs = new ConfigBase[guids.Length];

        for (var i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            configs[i] = AssetDatabase.LoadAssetAtPath<TT>(path);
        }

        Array.Sort(configs);
        configs = ResetId(configs);
        return configs;
    }

    private static ConfigBase[] ResetId(ConfigBase[] configs)
    {
        for (var i = 0; i < configs.Length; i++)
        {
            if (configs[i].Id != i)
            {
                configs[i].Id = i;
            }
        }

        return configs;
    }

    protected void DrawTitle(string editorName)
    {
        EditorHelper.DrawCenterLabel(editorName, 25, Color.white, FontStyle.Bold);
        var lastRect = GUILayoutUtility.GetLastRect();
        var area = new Rect(new Rect(0, lastRect.yMax, position.width, position.height - lastRect.yMax - 1));
        GUI.Box(area, GUIContent.none, GUI.skin.window);
    }

    protected void DrawConfig<TT>() where TT : ConfigBase
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSliderBar<TT>(Configs);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if (_selectedProperty != null)
        {
            foreach (var config in Configs)
            {
                if (config.Id.ToString() != _selectedProperty)
                {
                    continue;
                }

                SerializedObject = new SerializedObject(config);
                _serializedProperty = SerializedObject.GetIterator();
                _serializedProperty.NextVisible(true);

                GUILayout.BeginHorizontal();
                var area = GUILayoutUtility.GetRect(60, 60, GUIStyle.none, GUILayout.MaxWidth(60),
                    GUILayout.MaxHeight(60));
                GUI.Label(area, AssetPreview.GetAssetPreview(config.model));
                DrawProperties(_serializedProperty);
            }
        }
        else
        {
            EditorGUILayout.LabelField("왼쪽 리스트에서 몬스터를 선택 해주세요.");
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }

    private void DrawSliderBar<TT>(IEnumerable<ConfigBase> configBases) where TT : ConfigBase
    {
        foreach (var configBase in configBases)
        {
            var p = (TT) configBase;

            if (GUILayout.Button(p.Name))
            {
                _selectedPropertyName = p.Id.ToString();
            }
        }

        if (!string.IsNullOrEmpty(_selectedPropertyName))
        {
            _selectedProperty = _selectedPropertyName;
        }
    }

    protected void DrawButton()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            if (GUILayout.Button(" 생성하기", EditorStyles.toolbarButton, GUILayout.Width(65)))
            {
                CreateNewConfig();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(" 불러오기", EditorStyles.toolbarButton, GUILayout.Width(65)))
            {
            }

            if (GUILayout.Button(" 저장하기", EditorStyles.toolbarButton, GUILayout.Width(65)))
            {
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    protected static void DrawProperties(SerializedProperty p)
    {
        GUILayout.BeginVertical();
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

            if (p.name == "model")
            {
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
            }
        }

        GUILayout.EndVertical();
    }

    private void Apply()
    {
        SerializedObject.ApplyModifiedProperties();
    }

    protected virtual void CreateNewConfig()
    {
    }
}