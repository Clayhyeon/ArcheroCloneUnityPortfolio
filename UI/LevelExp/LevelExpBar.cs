using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelExpBar : MonoBehaviour
{
    public Slider expBar;
    public TextMeshProUGUI levelTxt;
    
    private void Start()
    {
        PlayerData.Instance.OnLevelUp += OnLevelUp;
        PlayerData.Instance.OnAddExp += OnAddExp;
    }

    private void OnAddExp()
    {
        var exp = (float) PlayerData.Instance.Exp * 100 / ExpTable.Instance.playerExpTable[PlayerData.Instance.Level + 1];
        expBar.value = exp;
    }

    private void OnLevelUp()
    {
        levelTxt.text = PlayerData.Instance.Level.ToString();
    }
}
