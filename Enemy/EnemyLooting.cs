using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooting : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float firingAngle = 45.0f;
    private const float Gravity = 9.8f;

    private Transform _projectile;
    private StageManager _stageManager;

    private int _money;
    private void Awake()
    {
        _projectile = transform;
    }

    public void Init(StageManager stageManager, int money)
    {
        _money = money;
        _stageManager = stageManager;
        _stageManager.OnStageEnd += Start_Looting;
    }
    
    private void Start_Looting(StageManager owner)
    {
        StartCoroutine(GoToPlayer());
    }
    
    private IEnumerator GoToPlayer()
    {
        yield return new WaitForSeconds(1f);
        _target = _stageManager.GetPlayer();
        
        var targetDistance = Vector3.Distance(_projectile.position, _target.position);
        var projectileVelocity = targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / Gravity);
        
        var vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        var vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
       
        var flightDuration = targetDistance / vx;
   
        _projectile.rotation = Quaternion.LookRotation(_target.position - _projectile.position);
       
        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            _projectile.Translate(0, (vy - (Gravity * elapse_time)) * Time.deltaTime, vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
 
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _stageManager.OnStageEnd -= Start_Looting;
            PlayerData.Instance.UpdateMoney(_money);
            GameManager.Pooling.ReturnObject(transform, "Money");
            SaveManager.Instance.WritePlayerData();
        }
    }
}
