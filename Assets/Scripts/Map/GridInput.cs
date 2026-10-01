using UnityEngine;

public class GridInput : MonoBehaviour
{
    public GridManager gridManager;
    public LayerMask gridLayer;

    public Cell GetPointedCell(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            gridLayer))
        {
            return null;
        }

        return gridManager.WorldToCell(hit.point);
    }
}
