using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Card SelectedCard { get; set; }
    public Cell SelectedCell { get; set; }

    public void TryPlayCard()
    {
        if (SelectedCard == null || SelectedCell == null)
            return;

        if (!SelectedCard.data.CanTarget(SelectedCell))
            return;

        SelectedCard.data.ApplyEffect(SelectedCell);

        SelectedCard = null;
        SelectedCell = null;
    }
}
