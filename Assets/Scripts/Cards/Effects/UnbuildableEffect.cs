using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Unbuildable")]
public class UnbuildableEffect : CardEffect
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
            if (cell != null)
                cell.IsBuildable = false;
        }
    }
}
