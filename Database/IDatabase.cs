using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IDatabase
{
    public void Init(LoadingManager loadingManager);
    public void Save(AssetBundle ab);

    
}
