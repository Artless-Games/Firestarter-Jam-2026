using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    // Singleton
    public static BuildingManager Instance { get; private set; }

    public GridManager gridManager;
    public Building buildingPrefab;
    public int buildingCount = 10;

    private readonly List<Building> buildings = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

    public void ProcessTurn()
    {
        //

        TurnManager.Instance.EndBuildingsPhase();
    }

    public IEnumerator ProcessFires()
    {
        List<Building> burningBuildings = new();

        foreach (Building building in buildings)
        {
            if (building != null && building.IsOnFire)
                burningBuildings.Add(building);
        }

        foreach (Building building in burningBuildings)
        {
            if (building != null && building.IsOnFire)
                yield return ProcessFire(building);
        }

        yield return null;
    }

    private IEnumerator ProcessFire(Building building)
    {
        Cell cell = building.Cell;

        if (cell == null)
            yield break;

        CardVFX fireVFX = null;

        int fireDamage = building.FireDamage;

        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int direction in directions)
        {
            Cell adjacentCell =
                GridManager.Instance.GetCell(
                    cell.Coordinates + direction);

            if (adjacentCell != null && adjacentCell.HasBuilding)
            {
                Building adjacentBuilding = adjacentCell.Building;

                adjacentBuilding.TakeDamage(fireDamage);

                if (adjacentBuilding != null)
                    fireVFX = adjacentBuilding.SetOnFire(fireDamage, building.FireVfxPrefab);
            }
        }

        if (fireVFX != null)
        {
            bool finished = false;

            void OnFinished()
            {
                finished = true;
            }

            fireVFX.Finished += OnFinished;

            yield return new WaitUntil(() => finished);

            fireVFX.Finished -= OnFinished;
        }

        building.Extinguish();
    }
}
