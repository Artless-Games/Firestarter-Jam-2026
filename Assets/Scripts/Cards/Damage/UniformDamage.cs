using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Damage/Uniform")]
public class UniformDamage : CardDamage
{
    public override void ApplyDamage(Cell origin, List<Cell> cells, DamageData damage)
    {
        foreach (Cell cell in cells)
        {
            if (cell != null && cell.HasBuilding)
                cell.Building.TakeDamage(damage.maxDamage);
        }
    }
}
