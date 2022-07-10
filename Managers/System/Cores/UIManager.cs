using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    public int Order = 10;

    public bool CanvasCreated = false;

    private Stack<GameObject> _popups; // 팝업 클릭했을 때 나오는 UI들을 담아둠, 먼저열린게 제일 나중에 닫히니 스택을 사용

    private GameObject _sceneUI; // 해당 씬에 해당하는 씬UI(기본 UI)를 저장

    private GameObject Root
    {
        get
        {
            var root = GameObject.Find("UI_ROOT");

            if (root != null)
            {
                return root;
            }

            root = new GameObject {name = "UI_ROOT"};
            var canvas = Object.Instantiate(UIDatabase.Instance.data["Canvas"].confing.model, root.transform, true);
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.GetComponent<Canvas>().overrideSorting = true;

            return root;
        }
    }

    public void Init()
    {
        _popups = new Stack<GameObject>();
    }

    public void ShowSceneUI(string sceneUIName)
    {
        _sceneUI = Object.Instantiate(UIDatabase.Instance.data[sceneUIName].confing.model,
            Root.transform.GetChild(0).transform, true);
        _sceneUI.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ShowPopupUI(string popupUIName)
    {
        var ui = Object.Instantiate(UIDatabase.Instance.data[popupUIName].confing.model,
            Root.transform.GetChild(0).transform, true);
        ui.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _popups.Push(ui);
    }

    public void ClosePopupUI()
    {
        if (_popups.Count < 1)
        {
            return;
        }

        Object.Destroy(_popups.Pop());
    }

    public void CloseSceneUI()
    {
        if (_sceneUI.activeSelf == true)
        {
            _sceneUI.SetActive(false);
        }
    }
}
