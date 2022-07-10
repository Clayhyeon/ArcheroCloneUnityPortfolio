using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private Dictionary<string, Queue<GameObject>> _soundPoolQueues;
    private Dictionary<string, GameObject> _soundPoolHierarchy;
    private Transform _soundPoolTransform;

    private int _soundPoolCount;
    
    private Dictionary<string, int> _addIndex;
    
    public void Init(int poolingCount)
    {
        _soundPoolCount = poolingCount;
        _soundPoolTransform = Find();

        _soundPoolQueues = new Dictionary<string, Queue<GameObject>>();
        _soundPoolHierarchy = new Dictionary<string, GameObject>();
        _addIndex = new Dictionary<string, int>();
        
        PoolingInit();
    }
    
    private Transform Find()
    {
        var obj = GameObject.Find("@Sound_List");

        if (obj is not null)
        {
            return obj.transform;
        }
        
        obj = new GameObject { name = "@Sound_List"};
        Object.DontDestroyOnLoad(obj);

        return obj.transform;
    }
    
    private void PoolingInit()
    {
        foreach (var names in SoundDatabase.Instance.data.Keys)
        {
            var obj = GameObject.Find(names);

            if (obj is null)
            {
                _soundPoolHierarchy.Add(names, new GameObject());
                _soundPoolHierarchy[names].name = names;
                _soundPoolHierarchy[names].transform.SetParent(_soundPoolTransform);
            }
            else
            {
                _soundPoolHierarchy.Add(names, obj);
            }
            
            _soundPoolQueues.Add(names, new Queue<GameObject>());

            for (var i = 0; i < _soundPoolCount; i++)
            {
                _soundPoolQueues[names].Enqueue(CreateSound(names, i));
            }

            _addIndex[names] = 0;
        }
    }
    
    private GameObject CreateSound(string name, int index)
    {
        var newObj = Object.Instantiate(SoundDatabase.Instance.data[name].confing.model,
            _soundPoolHierarchy[name].transform, true);
        
        Debug.Assert(newObj, $"SoundDatabase {name}에 해당하는 사운드가 존재하지 않습니다.");

        newObj.name = newObj.name + "Sound" +  " - " + index.ToString();
        newObj.SetActive(false);

        return newObj;
    }
    
    public void Play(Transform owner, string name)
    {
        Debug.Assert(_soundPoolQueues.ContainsKey(name), $"풀링 큐 안에 {name}이 존재하지 않습니다.");
        if (_soundPoolQueues[name].Count.Equals(0))
        {
            var newObj = CreateSound(name, ++_addIndex[name]);
            Debug.Assert(newObj, $"SoundDatabase에 {name}에 해당하는 게임 오브젝트가 존재하지 않습니다.");
            _soundPoolQueues[name].Enqueue(newObj);
        }

        var obj = _soundPoolQueues[name].Dequeue();
        Debug.Assert(obj, $"Queue에 {name}에 해당하는 게임 오브젝트가 존재하지 않습니다.");

        obj.transform.SetParent(null);
        obj.transform.position = owner.position;
        obj.transform.rotation = owner.rotation;
        obj.SetActive(true);
        
    }
    
    public void Stop(Transform owner, string name)
    {
        owner.gameObject.SetActive(false);
        owner.gameObject.transform.SetParent(_soundPoolHierarchy[name].transform);
        
        if (owner.gameObject != null)
        {
            _soundPoolQueues[name].Enqueue(owner.gameObject);
        }
    }

}
