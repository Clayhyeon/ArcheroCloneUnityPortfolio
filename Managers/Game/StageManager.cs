using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    #region Player

    [SerializeField] private Transform player;
    public bool IsPlayer => !player.GetComponent<ControllerBase>().dead && player.gameObject.activeSelf;

    private Transform _playerSpawn;

    #endregion

    #region Enemy

    public List<GameObject> enemies;
    public List<GameObject> deadEnemies;
    private int ThisStageEnemyCount => enemies.Count;
    public bool IsTarget => enemies.Count != 0;

    private Transform _target;
    private GameObject _targetEffect;

    private Transform _enemySpawn;

    #endregion

    #region StageManagement

    public Types.Stage stage;
    private int _stageCount;
    private int _stageIndex;

    private List<Transform> _stageList;
    private const char Split = ' ';

    public bool isStart;
    public bool isNext;

    public delegate void StageStageChangeHandler(StageManager owner);

    public event StageStageChangeHandler OnStageStart = null;
    public event StageStageChangeHandler OnStageEnd = null;

    #endregion

    private void Start()
    {
        isStart = false;
        Init();
        OnStageStart?.Invoke(this);
    }

    private void Update()
    {
        TargetEffect();
    }


    private void Init()
    {
        InitData();
        InitStage();
        GetEffect();
    }

    private void InitData()
    {
        PlayerData.Instance.OnLevelUp += LevelUp;
    }

    private void InitVariables()
    {
        stage = Types.Stage.Stage1;
        _stageIndex = 0;
        _stageList = new List<Transform>();
        enemies = new List<GameObject>();
        deadEnemies = new List<GameObject>();
    }

    private static void LevelUp()
    {
        GameManager.UI.ShowPopupUI("AddSkill");
    }

    private void CreatePlayer()
    {
        PlayerData.Instance.PlayerStatInit();
        
        _playerSpawn = GameObject.Find("PlayerSpawn").transform;
        
        player = Instantiate(PlayerData.Model, _playerSpawn.position, _playerSpawn.rotation).GetComponent<Transform>();
        player.GameObject().SetActive(false);
        
        player.GetComponent<PlayerController>()
            .Init(this, WeaponDatabase.Instance.data[PlayerData.Instance.EquipWeapon].confing);
        
        player.GetComponent<PlayerController>().OnStateDead += OnPlayerDead;
        
        player.gameObject.name = "Player";
        player.GameObject().SetActive(true);
    }

    private void FindStage()
    {
        var spawns = GameObject.Find(stage.ToString());
        _stageCount = spawns.transform.childCount;

        for (var i = 0; i < _stageCount; i++)
        {
            _stageList.Add(spawns.transform.GetChild(i));
        }
    }

    private void InitStage()
    {
        if (_stageList is null)
        {
            InitVariables();
        }

        if (_stageList.Count == 0)
        {
            FindStage();
        }

        SpawnEnemies();
        CreatePlayer();
    }

    private void SpawnEnemies()
    {
        if (_enemySpawn is not null)
        {
            _enemySpawn.parent.gameObject.SetActive(false);
        }

        _enemySpawn = _stageList[_stageIndex].transform.GetChild(0);
        _enemySpawn.parent.gameObject.SetActive(true);

        foreach (var enemy in _enemySpawn.GetComponentsInChildren<Transform>())
        {
            if (enemy.gameObject.name.Equals(_enemySpawn.gameObject.name))
            {
                continue;
            }

            var enemyName = enemy.gameObject.name.Split(Split)[1];
            var enemyObj = Instantiate(EnemyDatabase.Instance.data[enemyName].confing.model, enemy.position,
                enemy.rotation);

            enemyObj.SetActive(false);

            enemyObj.GetComponent<EnemyController>().Init(this, EnemyDatabase.Instance.data[enemyName].confing);
            enemyObj.GetComponent<EnemyController>().OnStateDead += OnEnemyDead;
            enemies.Add(enemyObj);

            enemyObj.SetActive(true);
        }
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public Transform GetTarget(Transform owner)
    {
        if (IsTarget is false)
        {
            return null;
        }

        var enemiesTr = new List<Transform>();

        for (var i = 0; i < enemies.Count; i++)
        {
            if (owner.transform == enemies[i].transform)
            {
                continue;
            }

            enemiesTr.Add(enemies[i].transform);
        }

        var enemiesDistance = enemiesTr.Select(t => Vector3.Distance(owner.position, t.position)).ToList();
        var targetIndex = enemiesDistance.ToList().IndexOf(enemiesDistance.Min());
        var target = enemiesTr[targetIndex];

        _targetEffect.transform.position = target.position;

        return target;
    }

    private IEnumerator NextCheck()
    {
        if (_stageIndex == _stageCount - 1)
        {
            EndStage();
        }
        else
        {
            CreateMoney();

            while (isNext)
            {
                yield return null;
            }


            deadEnemies.Clear();
            _stageIndex++;

            NextStage();
        }
    }

    private void NextStage()
    {
        player.transform.position = _playerSpawn.position;
        player.transform.rotation = _playerSpawn.rotation;
        player.GetComponent<PlayerController>().State = Types.State.Idle;

        SpawnEnemies();
        OnStageStart?.Invoke(this);
    }

    private void CreateMoney()
    {
        foreach (var dead in deadEnemies)
        {
            var money = GameManager.Pooling.GetObject(dead.transform.position, Quaternion.identity, "Money");
            money.GetComponent<EnemyLooting>().Init(this, dead.GetComponent<EnemyController>().enemyConfig.money);
            Destroy(dead);
        }
        
        OnStageEnd?.Invoke(this);
        isNext = true;
    }

    private void EndStage()
    {
        CreateMoney();
        StartCoroutine(ReturnLobby());
    }

    private IEnumerator ReturnLobby()
    {
        while (isNext)
        {
            yield return null;
        }
        
        SceneManager.LoadScene("Lobby");
        yield return null;
    }
    


    private void OnPlayerDead(ControllerBase owner)
    {
        Debug.Log("플레이어가 사망 하였습니다.");
    }

    private void OnEnemyDead(ControllerBase owner)
    {
        var enemyController = (EnemyController) owner;
        PlayerData.Instance.UpdateExp(enemyController.enemyConfig.exp);

        enemies.Remove(owner.gameObject);
        deadEnemies.Add(owner.gameObject);

        if (IsTarget is false && _targetEffect.activeSelf)
        {
            _targetEffect.SetActive(false);
        }

        if (ThisStageEnemyCount != 0)
        {
            return;
        }

        StartCoroutine(NextCheck());
    }

    private void GetEffect()
    {
        _targetEffect = Instantiate(PoolingDatabase.Instance.data["EnemyTarget"].confing.model);
        _targetEffect.SetActive(false);
    }

    private void TargetEffect()
    {
        if (!IsTarget)
        {
            return;
        }

        if (_targetEffect.activeSelf is false)
        {
            _targetEffect.SetActive(true);
        }

        _targetEffect.transform.position = GetTarget(player).position;
    }
}