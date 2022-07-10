using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private EnemyController _owner;

    private float _power;
    
    private void Start()
    {
        _owner = transform.root.GetComponent<EnemyController>();
        _power = _owner.enemyConfig.attackPower;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _owner.State == Types.State.Attack)
        {
            other.GetComponent<PlayerController>().UpdateDamaged(_power);
            
            GameManager.Pooling.GetObject(other.transform.position, transform.parent.transform.rotation, GetType().Name + "Hit");
            GameManager.Sound.Play(other.transform, GetType().Name + "HitSound");
        }

    }
}
