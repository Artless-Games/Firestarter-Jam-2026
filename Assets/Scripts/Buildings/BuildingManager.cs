using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    // Singleton
    public static BuildingManager Instance { get; private set; }

    public GridManager gridManager;
    public Building buildingPrefab;
    [SerializeField]
    private GameObject[] buildingModels;
    [SerializeField] private GameObject buildVfx;
    [SerializeField] private AudioClip buildSfx;

    [SerializeField] private GameObject destroyVfx;
    [SerializeField] private AudioClip destroySfx;

    [System.Serializable]
    public class BuildingTurnData
    {
        public int maxCost;
        public int tokens;
    }

    [SerializeField]
    private List<BuildingTurnData> turnData;

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

    private Building SpawnBuilding(Cell cell, int level)
    {
        Vector3 position = gridManager.CellToWorld(cell);

        Building building = Instantiate(
            buildingPrefab,
            position,
            Quaternion.identity,
            transform
        );

        building.Initialize(cell, this);
        building.SetHealth(level);

        UpdateBuildingModel(building);

        buildings.Add(building);

        PlayBuildEffects(building);

        return building;
    }

    public void UpdateBuildingModel(Building building)
    {
        if (building == null)
            return;

        for (int i = building.modelParent.childCount - 1; i >= 0; i--)
        {
            Destroy(building.modelParent.GetChild(i).gameObject);
        }

        int index = building.health - 1;

        if (index < 0 || index >= buildingModels.Length)
            return;

        Instantiate(
            buildingModels[index],
            building.modelParent.transform
        );
    }

    public void ProcessTurn()
    {
        BuildingTurnData data =
            turnData[TurnManager.Instance.Turn - 1];

        int tokens = data.tokens;
        int maxCost = data.maxCost;

        while (tokens > 0)
        {
            List<Cell> availableCells = GetAvailableCells();

            if (availableCells.Count == 0)
                break;

            Cell cell =
                availableCells[Random.Range(0, availableCells.Count)];

            int cost = GetBuildCost(tokens, maxCost);

            if (cost <= 0)
                break;

            SpawnBuilding(cell, cost);

            tokens -= cost;
        }

        TurnManager.Instance.EndBuildingsPhase();
    }

    private List<Cell> GetAvailableCells()
    {
        List<Cell> cells = new();

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int z = 0; z < gridManager.height; z++)
            {
                Cell cell = gridManager.GetCell(
                    new Vector2Int(x, z));

                if (cell != null &&
                    cell.IsBuildable &&
                    !cell.HasBuilding)
                {
                    cells.Add(cell);
                }
            }
        }

        return cells;
    }

    private int GetBuildCost(int tokens, int maxCost)
    {
        int highestCost = Mathf.Min(tokens, maxCost);

        if (highestCost <= 0)
            return 0;

        if (Random.value < 0.5f)
            return highestCost;

        if (highestCost == 1)
            return 1;

        return Random.Range(1, highestCost);
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

    private void PlayBuildEffects(Building building)
    {
        if (buildVfx != null)
        {
            Instantiate(
                buildVfx,
                building.transform.position,
                Quaternion.identity
            );
        }

        if (buildSfx != null)
        {
            AudioSource.PlayClipAtPoint(
                buildSfx,
                building.transform.position
            );
        }
    }
}
