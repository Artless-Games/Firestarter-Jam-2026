using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void ApplyEffect(
        Cell origin,
        List<Cell> cells,
        DamageData damageData,
        CardDamage damageType,
        GameObject vfx);
}
