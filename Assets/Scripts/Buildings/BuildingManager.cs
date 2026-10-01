using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GridManager gridManager;
    public Building buildingPrefab;
    public int buildingCount = 10;

    private readonly List<Building> buildings = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateBuildings();
    }

    public IReadOnlyList<Building> GetBuildings()
    {
        return buildings;
    }

    public void RemoveBuilding(Building building)
    {
        if (building == null)
            return;

        buildings.Remove(building);
    }

    public void ClearBuildings()
    {
        foreach (Building building in buildings)
        {
            if (building != null)
                Destroy(building.gameObject);
        }

        buildings.Clear();
    }

    private void GenerateBuildings()
    {
        int targetCount = Mathf.Min(buildingCount, gridManager.BuildableCellCount);

        int spawned = 0;

        while (spawned < targetCount)
        {
            int x = Random.Range(0, gridManager.width);
            int z = Random.Range(0, gridManager.height);

            Vector2Int coordinates = new Vector2Int(x, z);

            Cell cell = gridManager.GetCell(coordinates);

            if (cell != null &&
                cell.IsBuildable &&
                !cell.HasBuilding)
            {
                SpawnBuilding(cell);
                spawned++;
            }
        }
    }

    private void SpawnBuilding(Cell cell)
    {
        Vector3 position = gridManager.CellToWorld(cell);

        Building building = Instantiate(
            buildingPrefab,
            position,
            Quaternion.identity,
            transform
        );

        building.Initialize(cell, this);
        buildings.Add(building);
    }
}
