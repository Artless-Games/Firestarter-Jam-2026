using UnityEngine;

public class Building : MonoBehaviour
{
    public int health = 1;

    private BuildingManager buildingManager;

    public Cell Cell { get; private set; }
    public bool IsOnFire { get; private set; }
    public int FireDamage { get; private set; }
    public Transform modelParent;
    private GameObject fireVfxPrefab;
    private GameObject fireVfx;

    public GameObject FireVfxPrefab => fireVfxPrefab;

    public void Initialize(Cell cell, BuildingManager buildingManager)
    {
        Cell = cell;
        cell.Building = this;

        this.buildingManager = buildingManager;
    }

    public void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 1, 5);
    }

    public void Upgrade(int amount)
    {
        SetHealth(health + amount);
        buildingManager.UpdateBuildingModel(this);
    }

    public bool TakeDamage(int damage)
    {
        if (buildingManager != null)
            buildingManager.PlayDestroyEffects(this);

        health -= damage;

        if (health <= 0)
        {
            DestroyBuilding();
            return true;
        }

        buildingManager.UpdateBuildingModel(this);

        return false;
    }

    public CardVFX SetOnFire(int damage, GameObject vfxPrefab)
    {
        IsOnFire = true;
        FireDamage = damage;
        fireVfxPrefab = vfxPrefab;

        return StartFireVFX();
    }

    public CardVFX StartFireVFX()
    {
        if (fireVfx != null)
            return fireVfx.GetComponent<CardVFX>();

        if (fireVfxPrefab == null)
            return null;

        fireVfx = Instantiate(
            fireVfxPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );

        return fireVfx.GetComponent<CardVFX>();
    }

    public void Extinguish()
    {
        IsOnFire = false;
        FireDamage = 0;

        if (fireVfx != null)
        {
            Destroy(fireVfx);
            fireVfx = null;
        }

        fireVfxPrefab = null;
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
