using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Fire")]
public class FireEffect : CardEffect
{
    public override void ApplyEffect(
        Cell origin,
        List<Cell> cells,
        DamageData damageData,
        CardDamage damageType,
        GameObject vfx)
    {
        foreach (Cell cell in cells)
        {
            if (cell != null && cell.HasBuilding)
                cell.Building.SetOnFire(damageData.maxDamage, vfx);
        }
    }
}
