using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class AddressablesManager : MonoBehaviour
{
    #region Donwload UI

    public Slider _downloadSlider;
    public TextMeshProUGUI _downloadSize;
    public TextMeshProUGUI _downloadPercent;
    public GameObject _downloadUI;
    
    #endregion
    
    #region Addressables

    private AsyncOperationHandle _handle;
    private bool _isClear;
    private string _labelName;
    
    #endregion
    
    private const char Split = ' ';
    
    public void Load(bool isClear, string labelName, Action<AssetBundle> save)
    {
        _isClear = isClear;
        _labelName = labelName.Split(Split)[0];
        Observable.FromCoroutineValue<AssetBundle>(DownLoad).Subscribe(save);
    }
    
    private IEnumerator DownLoad()
    {
        if (_isClear)
        {
            var clearAsync = Addressables.ClearDependencyCacheAsync(_labelName, false);
            yield return clearAsync;
            Addressables.Release(clearAsync);
        }

        long updateLabelSize = 0;
        var sizeAsync = Addressables.GetDownloadSizeAsync(_labelName);
        yield return sizeAsync; // 다운로드할 사이즈를 가져올 때 까지 대기

        if (sizeAsync.Status == AsyncOperationStatus.Succeeded)
        {
            updateLabelSize = sizeAsync.Result; // 다운로드 사이즈를 불러오는데 성공했으면 캐싱함.
        }

        Addressables.Release(sizeAsync);


        _downloadUI.SetActive(true);
        _downloadSize.text = updateLabelSize.ToString() + " byte";
        _handle = Addressables.DownloadDependenciesAsync(_labelName, false);

        while (!_handle.IsDone)
        {
            var percent = _handle.PercentComplete;

            percent *= 100;
            percent = (float) (Math.Truncate(percent * 10) / 10);

            _downloadPercent.text = $"{percent}%";
            _downloadSlider.value = percent;

            yield return new WaitForEndOfFrame();
        }

        if (_handle.IsDone)
        {
            _downloadPercent.text = "100%";
            _downloadSlider.value = 1;
        }


        foreach (var ab in from item in (List<IAssetBundleResource>) _handle.Result select item.GetAssetBundle())
        {
            yield return ab;
        }


        yield return null;
    }
}
