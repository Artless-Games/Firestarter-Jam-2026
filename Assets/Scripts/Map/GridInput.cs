using UnityEngine;
using UnityEngine.InputSystem;

public class GridInput : MonoBehaviour
{
    public InputManager inputManager;
    public GridManager gridManager;
    public LayerMask gridLayer;

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 screenPosition = context.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayer))
            {
                Cell cell = gridManager.WorldToCell(hit.point);

                if (cell != null)
                {
                    GameManager.Instance.PointCell(cell);
                    return;
                }
            }

            GameManager.Instance.ClearCellHighlight();
        }

        else if (context.canceled)
        {
            GameManager.Instance.ClearCellHighlight();
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        GameManager.Instance.SelectPointedCell();
    }
}
