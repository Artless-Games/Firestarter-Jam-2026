using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum InputMode
    {
        Card,
        Grid
    }

    public CardInput cardInput;
    public GridInput gridInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetMode(InputMode.Card);
    }

    public void SetMode(InputMode mode)
    {
        cardInput.enabled = mode == InputMode.Card;
        gridInput.enabled = mode == InputMode.Grid;
    }
}
