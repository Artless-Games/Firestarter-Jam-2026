using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Damage/Column Falloff")]
public class ColumnFalloffDamage : CardDamage
{
    public override void ApplyDamage(Cell origin, List<Cell> cells, DamageData damage)
    {
        foreach (Cell cell in cells)
        {
            if (cell != null && cell.HasBuilding)
            {
                int distance = Mathf.Abs(
                cell.Coordinates.x - origin.Coordinates.x);

                int value = Mathf.Max(
                    damage.minDamage,
                    damage.maxDamage - distance);

                cell.Building.TakeDamage(value);
            }
        }
    }
}
