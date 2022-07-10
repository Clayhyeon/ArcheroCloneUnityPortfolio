using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigBase : ScriptableObject, IComparable
{
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string Name { get; set; }
    
    public GameObject model;

    
    public int CompareTo(object obj)
    {
        var configBase = (ConfigBase) obj;
        return Id.CompareTo((configBase.Id));
    }
    
}
