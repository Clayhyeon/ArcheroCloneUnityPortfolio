using UnityEngine;


public class EnemyDatabase : DatabaseBase<EnemyDatabase>, IDatabase
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
        Find<Enemy>();
        OnDownLoadCompleted += loadingManager.DownLoadCompleted;
    }
}
