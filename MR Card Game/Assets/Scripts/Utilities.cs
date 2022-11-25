using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Meteor,
    ArrowRain,
    ThunderStrike,
    Armor,
    Healing,
    Obliteration,
    Draw,
    Teleport,
    SpaceDistortion,
    SlowTime,
    StopTime,
    Rain
}
public enum TowerType
{
    Archer,
    Earth,
    Fire,
    Lightning,
    Wind
}

public enum EnemyType
{
    Normal,
    Fast,
    SuperFast,
    Flying,
    Tank,
    Slow,
    Berzerker,
    BerzerkerFlying,
    BerzerkerTank
}

public enum ResistenceAndWeaknessType
{
    None,
    Archer,
    Earth,
    Fire,
    Lighting,
    Wind
}

public enum TrapType
{
    Hole,
    Swamp
}