using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class DatabaseBase<T> : ScriptableObject where T : ScriptableObject, IDatabase
{
    #region Instance
    
    private const string ListFileDirectory = "Assets/Resources/ScriptableObjects/Databases";
    private static readonly string ListFilePath = "Assets/Resources/ScriptableObjects/Databases/" + typeof(T).Name + ".asset";
    
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance is not null)
            {
                return _instance;
            }

            _instance = Resources.Load<T>("ScriptableObjects/Databases/" + typeof(T).Name);
            
#if UNITY_EDITOR

            if (_instance is not null)
            {
                return _instance;
            }

            if (!AssetDatabase.IsValidFolder(ListFileDirectory))
            {
                AssetDatabase.CreateFolder("Assets/Resources/ScriptableObjects", "Database");
                
            }

            _instance = AssetDatabase.LoadAssetAtPath<T>(ListFilePath);
            
            if (_instance is not null)
            {
                return _instance;
            }

            _instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(_instance, ListFilePath);
#endif

            return _instance;
        }
    }

    [Serializable]
    public class DataDic : SerializableDictionary<string, Data>
    {
    }
    
    #endregion
    
    public delegate void DownloadHandler(ScriptableObject owner);
    public event DownloadHandler OnDownLoadCompleted;
    
    public DataDic data;
    
    [SerializeField] protected bool isClear; // 존재하는 데이터를 지우고 새로 다운로드 여부를 결정
    
    protected void DownloadCompleted()
    {
        OnDownLoadCompleted?.Invoke(this);
    }
    
    protected void Find<TT>() where TT : Data
    {
        var objs = Resources.LoadAll("ScriptableObjects/Data/" + typeof(TT).Name);

        foreach (var obj in objs)
        {
            var asset = (TT) obj;
            var overlap = false;

            if (data.Count != 0)
            {
                foreach (var item in data.Where(item => data[item.Key].confing.Id == asset.confing.Id))
                {
                    overlap = true;
                }
            }

            if (overlap is false)
            {
                data[asset.confing.Name] = asset;
            }
        }
// #if UNITY_EDITOR
//         var guids = AssetDatabase.FindAssets($"t:{typeof(TT)}");
//         foreach (var guid in guids)
//         {
//             var path = AssetDatabase.GUIDToAssetPath(guid);
//             var resource = AssetDatabase.LoadAssetAtPath<TT>(path);
//
//             if (resource.GetType() == typeof(TT))
//             {
//                 var overlap = false;
//
//                 if (data.Count != 0)
//                 {
//                     foreach (var item in data.Where(item => data[item.Key].confing.Id == resource.confing.Id))
//                     {
//                         overlap = true;
//                     }
//                 }
//
//                 if (overlap is false)
//                 {
//                     data[resource.confing.Name] = resource;
//                 }
//             }
//
//             EditorUtility.SetDirty(this);
//             AssetDatabase.SaveAssets();
//         }
// #endif
    }


#if UNITY_EDITOR
    protected static class AssetBundleEditorUtil
    {
        public static void FixShadersForEditor(GameObject prefab)
        {
            var renderers = prefab.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                ReplaceShaderForEditor(renderer.sharedMaterials);
            }

            var tmps = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var tmp in tmps)
            {
                ReplaceShaderForEditor(tmp.material);
                ReplaceShaderForEditor(tmp.materialForRendering);
            }

            var spritesRenderers = prefab.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var spriteRenderer in spritesRenderers)
            {
                ReplaceShaderForEditor(spriteRenderer.sharedMaterials);
            }

            var images = prefab.GetComponentsInChildren<Image>(true);
            foreach (var image in images)
            {
                ReplaceShaderForEditor(image.material);
            }

            var particleSystemRenderers = prefab.GetComponentsInChildren<ParticleSystemRenderer>(true);
            foreach (var particleSystemRenderer in particleSystemRenderers)
            {
                ReplaceShaderForEditor(particleSystemRenderer.sharedMaterials);
            }

            var particles = prefab.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var particle in particles)
            {
                var renderer = particle.GetComponent<Renderer>();

                if (renderer != null)
                {
                    ReplaceShaderForEditor(renderer.sharedMaterials);
                }
            }
        }

        private static void ReplaceShaderForEditor(Material[] materials)
        {
            foreach (var t in materials)
            {
                ReplaceShaderForEditor(t);
            }
        }

        private static void ReplaceShaderForEditor(Material material)
        {
            if (material is null)
            {
                return;
            }

            var shaderName = material.shader.name;
            var shader = Shader.Find(shaderName);

            if (shader != null)
            {
                material.shader = shader;
            }
        }
    }
#endif
}