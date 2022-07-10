using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types
{
    public enum EnemyCategory
    {
        Default,
        Boss
    }
    public enum EnemyAttackType
    {
        Melee,
        Range
    }

    [Serializable]
    public enum State
    {
        Idle,
        Movement,
        Attack,
        Dead
    }

    [Serializable]
    public enum WeaponType
    {
        Melee,
        Range
    }

    [Serializable]
    public enum PoolingType
    {
        Model,
        Effect,
    }

    [Serializable]
    public enum SoundType
    {
        Background,
        Effect,
        Ui
    }

    [Serializable]
    public enum UIType
    {
        Scene,
        Popup
    }

    public enum Stage
    {
        Stage1,
        Stage2
    }

    public enum Skill
    {
        FrontArrow,
        MultiShot,
        Ricochet
    }
    
}
