using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Chain")]
public class ChainEffect : CardEffect
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
            if (cell != null && !cell.HasBuilding)
                ApplyChain(cell, damageData);
        }
    }

    private void ApplyChain(Cell cell, DamageData damageData)
    {
        List<Building> adjacentBuildings = GetAdjacentBuildings(cell);

        if (adjacentBuildings.Count == 0)
            return;

        Building nextBuilding = adjacentBuildings[Random.Range(0, adjacentBuildings.Count)];

        Cell nextCell = nextBuilding.Cell;

        bool destroyed = nextBuilding.TakeDamage(damageData.maxDamage);

        if (!destroyed)
            return;

        ApplyChain(nextCell, damageData);
    }

    private List<Building> GetAdjacentBuildings(Cell cell)
    {
        List<Building> buildings = new();

        Vector2Int coordinates = cell.Coordinates;

        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int direction in directions)
        {
            Cell adjacentCell = GridManager.Instance.GetCell(coordinates + direction);

            if (adjacentCell != null &&
                adjacentCell.HasBuilding)
            {
                buildings.Add(adjacentCell.Building);
            }
        }

        return buildings;
    }
}
