using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Line")]
public class LineEffect : CardEffect
{
    public Vector2Int direction = Vector2Int.right;

    public override void ApplyEffect(
        Cell origin,
        List<Cell> cells,
        DamageData damageData,
        CardDamage damageType,
        GameObject vfx)
    {
        LineEffectManager.Instance.AddEffect(
            origin,
            direction,
            damageData,
            damageType,
            vfx);
    }
}
