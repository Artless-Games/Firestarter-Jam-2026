using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Damage/Random")]
public class RandomDamage : CardDamage
{
    public override void ApplyDamage(Cell origin, List<Cell> cells, DamageData damage)
    {
        foreach (Cell cell in cells)
        {
            if (cell != null && cell.HasBuilding)
            {
                int value = Random.Range(
                    damage.minDamage,
                    damage.maxDamage + 1);

                cell.Building.TakeDamage(value);
            }
        }
    }
}
