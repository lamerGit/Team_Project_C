using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���¿� Enum
public enum MonsterState
{
    Chase = 0,
    Attack,
    Dead
}

public enum MonsterType
{
    Nomal,
    Boss
}

public enum WeaponType
{
    NomalWeapon,
    BossWeapon
}

public enum ItemIDCode
{
    HP_potion = 0,
    Healing_Artifact,
    Trap
}
