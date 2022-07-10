using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject equipUI;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private string currentWeaponName;
    [SerializeField] private TextMeshProUGUI money;
    
    private int _weaponIndex;
    public int WeaponIndex
    {
        get => _weaponIndex;
        set
        {
            if (value < 0 || value == WeaponDatabase.Instance.data.Count)
            {
                Debug.Log("없는무기!!");
                return;
            }
            
            Show_Weapon(value);
            _weaponIndex = value;
            Check_Owner(value);
        }
    }

    private void Start()
    {
        UpdateMoney();
    }

    private void Show_Weapon(int nextIndex)
    {
        equipUI.transform.GetChild(WeaponIndex).gameObject.SetActive(false);
        equipUI.transform.GetChild(nextIndex).gameObject.SetActive(true);
        
        currentWeaponName = equipUI.transform.GetChild(nextIndex).gameObject.name;
    }

    public void Equip_Weapon()
    {
        PlayerData.Instance.EquipWeapon = currentWeaponName;
        SaveManager.Instance.WritePlayerData();
    }

    public void Buy_Weapon()
    {
        PlayerData.Instance.Weapons.Add(currentWeaponName);
        PlayerData.Instance.Money -= 100;
        
        UpdateMoney();
        Check_Owner(WeaponIndex);
        
        SaveManager.Instance.WritePlayerData();
    }

    private void Check_Owner(int index)
    {
        buyButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        
        var button = PlayerData.Instance.Weapons.Contains(equipUI.transform.GetChild(index).gameObject.name)
            ? equipButton
            : buyButton;
        
        button.gameObject.SetActive(true);
    }
    
    public void Before_Weapon()
    {
        WeaponIndex = WeaponIndex - 1;
    }

    public void Next_Weapon()
    {
        WeaponIndex = WeaponIndex + 1;
    }

    private void UpdateMoney()
    {
        money.text = PlayerData.Instance.Money.ToString();
    }

    public void MoveStage(string stageName)
    {
        SceneManager.LoadScene(stageName);
    }
    
    
}
