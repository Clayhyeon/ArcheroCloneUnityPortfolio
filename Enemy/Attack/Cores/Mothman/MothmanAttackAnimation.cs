using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothmanAttackAnimation : EnemyAttackAnimation
{

    private CharacterParts _parts;
    
    public override void AttackStart()
    {
        _parts ??= Controller.GetComponent<CharacterParts>();

        var player = Sm.GetPlayer();
        
        var obj = GameManager.Pooling.GetObject(_parts.rightHand.position, Quaternion.identity, "MothmanAttack");
        obj.GetComponent<Projectile>().Init(Controller.enemyConfig.attackPower, Controller.enemyConfig.attackSpeed);

        var lookPos = player.position - obj.transform.position;
        lookPos.y = 0f;
        var rot = Quaternion.LookRotation(lookPos);
        obj.transform.rotation = rot;
    }

}
