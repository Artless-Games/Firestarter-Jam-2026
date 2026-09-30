using UnityEngine;
using UnityEngine.InputSystem;

public class CardInput : MonoBehaviour
{
    public InputManager inputManager;
    public LayerMask cardLayer;

    private Card currentPointedCard;
    private Card selectedCard;

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (currentPointedCard != null)
            {
                currentPointedCard.OnPoint(false);
                currentPointedCard = null;
            }

            return;
        }

        Vector2 screenPosition = context.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        Card newPointedCard = null;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cardLayer))
        {
            newPointedCard = hit.collider.GetComponent<Card>();
        }

        if (newPointedCard == currentPointedCard)
            return;

        if (currentPointedCard != null)
            currentPointedCard.OnPoint(false);

        currentPointedCard = newPointedCard;

        if (currentPointedCard != null)
            currentPointedCard.OnPoint(true);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (currentPointedCard == null)
            return;

        if (selectedCard == currentPointedCard)
        {
            selectedCard.OnSelect(false);
            selectedCard = null;
            return;
        }

        if (selectedCard != null)
            selectedCard.OnSelect(false);

        selectedCard = currentPointedCard;
        selectedCard.OnSelect(true);

        GameManager.Instance.SelectedCard = selectedCard;

        currentPointedCard.OnPoint(false);
        currentPointedCard = null;

        inputManager.SetMode(
            InputManager.InputMode.Grid
        );
    }
}
