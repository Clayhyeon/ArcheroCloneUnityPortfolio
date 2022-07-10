using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTable : Table<SkillTable>
{
    [Serializable]
    public class SkillTableDic : SerializableDictionary<Types.Skill, Skill>
    {
        
    }

    [SerializeField] public SkillTableDic playerSkillTable;
}
