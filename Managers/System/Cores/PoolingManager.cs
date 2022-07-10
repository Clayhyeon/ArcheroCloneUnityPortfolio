using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager
{
    private Dictionary<string, Queue<GameObject>> _objPoolQueues;
    private Dictionary<string, GameObject> _objPoolHierarchy;
    private Transform _objPoolTransform;

    private int _objPoolCount;
    
    private Dictionary<string, int> _addIndex;

    public void Init(int poolingCount)
    {
        _objPoolCount = poolingCount;
        _objPoolTransform = Find();

        _objPoolQueues = new Dictionary<string, Queue<GameObject>>();
        _objPoolHierarchy = new Dictionary<string, GameObject>();
        _addIndex = new Dictionary<string, int>();
        
        PoolingInit();
    }

    private Transform Find()
    {
        var obj = GameObject.Find("@Pooling_List");

        if (obj is not null)
        {
            return obj.transform;
        }
        
        obj = new GameObject {name = "@Pooling_List"};
        Object.DontDestroyOnLoad(obj);

        return obj.transform;
    }
    
    private void PoolingInit()
    {
        foreach (var names in PoolingDatabase.Instance.data.Keys)
        {
            var obj = GameObject.Find(names);

            if (obj is null)
            {
                _objPoolHierarchy.Add(names, new GameObject());
                _objPoolHierarchy[names].name = names;
                _objPoolHierarchy[names].transform.SetParent(_objPoolTransform);
            }
            else
            {
                _objPoolHierarchy.Add(names, obj);
            }
            
            _objPoolQueues.Add(names, new Queue<GameObject>());

            for (var i = 0; i < _objPoolCount; i++)
            {
                _objPoolQueues[names].Enqueue(CreateObject(names, i));
            }

            _addIndex[names] = 0;
        }
    }

    private GameObject CreateObject(string name, int index)
    {
        var newObj = Object.Instantiate(PoolingDatabase.Instance.data[name].confing.model,
            _objPoolHierarchy[name].transform, true);
        
        Debug.Assert(newObj, $"PoolingDatabase에 {name}에 해당하는 게임 오브젝트가 존재하지 않습니다.");

        newObj.name = newObj.name + " - " + index.ToString();
        newObj.SetActive(false);

        return newObj;
    }

    public GameObject GetObject(Vector3 pos, Quaternion rot, string name)
    {
        Debug.Assert(_objPoolQueues.ContainsKey(name), $"풀링 큐 안에 {name}이 존재하지 않습니다.");
        
        if (_objPoolQueues[name].Count.Equals(0))
        {
            var newObj = CreateObject(name, ++_addIndex[name]);
            Debug.Assert(newObj, $"PoolingDatabase에 {name}에 해당하는 게임 오브젝트가 존재하지 않습니다.");
            _objPoolQueues[name].Enqueue(newObj);
        }

        var obj = _objPoolQueues[name].Dequeue();
        Debug.Assert(obj, $"Queue에 {name}에 해당하는 게임 오브젝트가 존재하지 않습니다.");

        obj.transform.SetParent(null);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(Transform owner, string name)
    {
        owner.gameObject.SetActive(false);
        owner.gameObject.transform.SetParent(_objPoolHierarchy[name].transform);

        if (owner.GetComponent<Projectile>())
        {
            owner.GetComponent<Projectile>().Reset_Projectile();
        }
        
        if (owner.gameObject != null)
        {
            _objPoolQueues[name].Enqueue(owner.gameObject);
        }
    }
}
