using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType
{
    Sound,
    Effect
}
public class ReturnPoolObject : MonoBehaviour
{
    [SerializeField] private string objName;
    public ObjType type;
    
    private void OnEnable()
    {
        StartCoroutine(Stop());
    }

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(1f);
        if (type == ObjType.Sound)
        {
            GameManager.Sound.Stop(transform, objName);
        }

        if (type == ObjType.Effect)
        {
            GameManager.Pooling.ReturnObject(transform, objName);
        }
    }
}
