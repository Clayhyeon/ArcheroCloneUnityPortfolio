using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerData : Singleton<PlayerData>
{
    public string userId;

    private GameObject _model;

    public static GameObject Model
    {
        get => Instance._model;
        private set => Instance._model = value;
    }

    private int _level;
    private int _exp;
    private int _money;
    private string _modelName;
    private string _equipWeapon;

    private List<string> _modelNames = new List<string>();
    private List<string> _weapons = new List<string>();
    private readonly List<Skill> _skills = new List<Skill>();

    public int Level
    {
        get => Instance._level;
        set => Instance._level = value;
    }

    public int Exp
    {
        get => Instance._exp;
        set
        {
            Instance._exp = value;

            if (Exp == 0)
            {
                return;
            }

            if (Exp < ExpTable.Instance.playerExpTable[Level + 1])
            {
                OnAddExp?.Invoke();
                return;
            }

            Exp -= ExpTable.Instance.playerExpTable[Level + 1];
            OnAddExp?.Invoke();
            Level++;
            OnLevelUp?.Invoke();
        }
    }

    public int Money
    {
        get => Instance._money;
        set => Instance._money = value;
    }

    private string ModelName
    {
        get => Instance._modelName;
        set
        {
            Instance._modelName = value;
            Model = PlayerCharacterDatabase.Instance.data[value].confing.model;
        }
    }

    public string EquipWeapon
    {
        get => Instance._equipWeapon;
        set => Instance._equipWeapon = value;
    }

    public List<string> ModelNames
    {
        get => Instance._modelNames;
        private set => Instance._modelNames = value.ToList();
    }
    public List<string> Weapons
    {
        get => Instance._weapons;
        private set => Instance._weapons = value.ToList();
    }

    public List<Skill> Skills => Instance._skills;

    #region Events

    public delegate void PlayerDataChangeHandler();

    public event PlayerDataChangeHandler OnLevelUp = null;
    public event PlayerDataChangeHandler OnAddExp = null;

    #endregion

    public void PlayerStatInit()
    {
        Level = 1;
        Exp = 0;
        Skills.Clear();
    }

    public void PlayerDataInit()
    {
        ModelName = "Default";
        ModelNames.Add("Default");

        Weapons.Add("Bow");
        EquipWeapon = "Bow";

        Money = 0;
    }

    public Dictionary<string, object> GetPlayerData()
    {
        var userInfo = new Dictionary<string, object>
        {
            {"Money", Money}, {"Weapons", Weapons}, {"EquipWeapon", EquipWeapon}, {"ModelNames", ModelNames},
            {"ModelName", ModelName}
        };
        return userInfo;
    }

    public void SetPlayerData(Dictionary<string, object> playerDic)
    {
        Debug.Log("로드 Data 맵핑 시작");


        Money = int.Parse(playerDic["Money"].ToString());
        EquipWeapon = playerDic["EquipWeapon"].ToString();
        ModelName = playerDic["ModelName"].ToString();

        Debug.Log("3개 완료");

        Debug.Log(playerDic["Weapons"].ConvertTo(typeof(List<string>)));
        
        foreach (var weapon in (IEnumerable) playerDic["Weapons"] )
        {
            Weapons.Add(weapon.ToString());
        }
        
        foreach (var model in (IEnumerable) playerDic["ModelNames"] )
        {
            ModelNames.Add(model.ToString());
        }
        
        Debug.Log("로드 Data 맵핑 끝");
    }

    public void UpdateExp(int exp)
    {
        Exp += exp;
    }

    public void UpdateMoney(int money)
    {
        Debug.Log($"{money}원을 획득 하셨습니다.");
        Money += money;
        Debug.Log($"현재금액 : {Money}");
    }

    public void AddSkill(Types.Skill skill)
    {
        if (Skills.Contains(SkillTable.Instance.playerSkillTable[skill]))
        {
            return;
        }

        Skills.Add(SkillTable.Instance.playerSkillTable[skill]);
    }
}