using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

[RequireComponent(typeof(AddressablesManager))]

public class LoadingManager : MonoBehaviour
{
    private List<IDatabase> _databaseBases;
    public List<ScriptableObject> dbList;

    private AddressablesManager _manager;

    private int DBCount => dbList.Count;
    private int _currentDB;

    [SerializeField] private GameObject googleLoginButton;
    [SerializeField] private GameObject startText;

    private string _beforeOwner;
    
    public void StartMain()
    {
        startText.SetActive(true);
    }
    
    public void Init()
    {
        _manager = GetComponent<AddressablesManager>();
        
        FindDatabase();
        
        foreach (var db in _databaseBases)
        {
            db.Init(this);
        }
        
        StartCoroutine(Download(_currentDB));
    }
    private void FindDatabase()
    {
        _databaseBases = new List<IDatabase>();
        var databases = Resources.LoadAll("ScriptableObjects/Databases");

        foreach (var database in databases)
        {
            _databaseBases.Add((IDatabase) database);
            dbList.Add(_databaseBases.Last() as ScriptableObject);
        }
    }

    private IEnumerator Download(int index)
    {
        yield return new WaitForSeconds(1f);
        _manager.Load(true, _databaseBases[index].ToString(), _databaseBases[index].Save);
    }
    
    public void DownLoadCompleted(ScriptableObject owner)
    {
        if (_beforeOwner == owner.name)
        {
            return;
        }
        
        _currentDB++;
        _beforeOwner = owner.name;
        
        if (_currentDB == DBCount)
        {
            GameManager.Instance.ManagerInit();
            ShowLogin();
        }
        else
        {
            StartCoroutine(Download(_currentDB));
        }
    }

    private void ShowLogin()
    {
        _manager._downloadUI.SetActive(false);
        googleLoginButton.SetActive(true);
    }
    

    
    
}

