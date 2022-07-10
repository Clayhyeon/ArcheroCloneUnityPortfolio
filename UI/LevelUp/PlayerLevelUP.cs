using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelUP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    private void OnEnable()
    {
        levelText.text = PlayerData.Instance.Level.ToString();
    }
}
