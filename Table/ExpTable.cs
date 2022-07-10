using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExpTable : Table<ExpTable>
{
    [Serializable]
    public class ExpTableDic : SerializableDictionary<int, int>
    {
        
    }

    [SerializeField] public ExpTableDic playerExpTable;
}
