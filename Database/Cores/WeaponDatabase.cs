using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDatabase : DatabaseBase<WeaponDatabase>, IDatabase
{
    public void Save(AssetBundle ab)
    {
        foreach (var obj in ab.LoadAllAssets<GameObject>())
        {
#if UNITY_EDITOR
            AssetBundleEditorUtil.FixShadersForEditor(obj);
#endif
            data[obj.name].confing.model = obj;
        }
        
        DownloadCompleted();
    }
    
    public void Init(LoadingManager loadingManager)
    {
        Find<Weapon>();
        OnDownLoadCompleted += loadingManager.DownLoadCompleted;
    }
}

