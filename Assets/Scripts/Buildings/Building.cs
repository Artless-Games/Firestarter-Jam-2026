using UnityEngine;

public class Building : MonoBehaviour
{
    public int health = 100;

    public Cell Cell { get; private set; }

    private BuildingManager buildingManager;

    public void Initialize(Cell cell, BuildingManager buildingManager)
    {
        Cell = cell;
        cell.Building = this;

        this.buildingManager = buildingManager;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyBuilding();
        }
    }

    public void DestroyBuilding()
    {
        if (Cell != null)
        {
            Cell.RemoveBuilding();
            Cell = null;
        }

        if (buildingManager != null)
        {
            buildingManager.RemoveBuilding(this);
            buildingManager = null;
        }

        Destroy(gameObject);
    }
}
