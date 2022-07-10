using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPick : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
        Show_SKill();
    }

    private void Show_SKill()
    {
        for (var i = 0; i < PlayerData.Instance.Skills.Count; i++)
        {
            transform.GetChild((int) PlayerData.Instance.Skills[i].type).gameObject.SetActive(false);
        }
    }

    public void Pick_Skill(string skillName)
    {
        PlayerData.Instance.AddSkill( (Types.Skill) Enum.Parse(typeof(Types.Skill), skillName) );
        Close();
    }

    private void Close()
    {
        Time.timeScale = 1;
        GameManager.UI.ClosePopupUI();
    }
}
