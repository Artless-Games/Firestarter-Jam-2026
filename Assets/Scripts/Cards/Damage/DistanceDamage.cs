using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Damage/Distance")]
public class DistanceDamage : CardDamage
{
    public override void ApplyDamage(Cell origin, List<Cell> cells, DamageData damage)
    {
        foreach (Cell cell in cells)
        {
            if (cell != null && cell.HasBuilding)
            {
                int distance = Mathf.Max(
                    Mathf.Abs(cell.Coordinates.x - origin.Coordinates.x),
                    Mathf.Abs(cell.Coordinates.y - origin.Coordinates.y)
            );

                int cellDamage = Mathf.Max(damage.minDamage, damage.maxDamage - distance);

                cell.Building.TakeDamage(cellDamage);
            }
        }
    }
}
