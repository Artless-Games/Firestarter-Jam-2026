using TMPro;
using UnityEngine;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private TMP_Text deckCountText;

    private void Update()
    {
        deckCountText.text =
            CardManager.Instance.DeckCount.ToString();
    }
}
