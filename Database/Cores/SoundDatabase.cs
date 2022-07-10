using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDatabase : DatabaseBase<SoundDatabase>, IDatabase
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
        Find<Sound>();
        OnDownLoadCompleted += loadingManager.DownLoadCompleted;
    }
}
