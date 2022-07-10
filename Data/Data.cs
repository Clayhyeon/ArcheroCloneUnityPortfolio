using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Data : ScriptableObject
{
    public ConfigBase confing;
    protected abstract void Init();
}
