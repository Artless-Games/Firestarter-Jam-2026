using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public MapManager mapManager;
    public LayerMask gridLayer;

    private Cell currentPointedCell;
    private Cell selectedCell;

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (currentPointedCell != null)
            {
                currentPointedCell.OnPoint(false);
                currentPointedCell = null;
            }

            return;
        }

        Vector2 screenPosition = context.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        Cell newCell = null;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayer))
        {
            newCell = mapManager.WorldToCell(hit.point);
        }

        if (newCell == currentPointedCell)
            return;

        if (currentPointedCell != null)
            currentPointedCell.OnPoint(false);

        currentPointedCell = newCell;

        if (currentPointedCell != null)
            currentPointedCell.OnPoint(true);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (currentPointedCell == null)
            return;

        if (selectedCell == currentPointedCell)
        {
            selectedCell.OnSelect(false);
            selectedCell = null;

            return;
        }

        if (selectedCell != null)
            selectedCell.OnSelect(false);

        selectedCell = currentPointedCell;
        selectedCell.OnSelect(true);
    }
}
