using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // Singleton
    public static TurnManager Instance { get; private set; }

    public enum TurnPhase
    {
        Buildings,
        Player
    }

    [Serializable]
    public class PlayerTurnData
    {
        public int energy;
        public int maxCardCost;
    }

    public int maxTurns = 5;
    public Button endTurnButton;

    [SerializeField]
    private List<PlayerTurnData> playerTurnData;

    public int Energy { get; private set; }
    public int MaxCardCost { get; private set; }

    public TurnPhase Phase { get; private set; }
    public int Turn { get; private set; } = 1;
    public bool IsProcessingPlayerPhase { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        endTurnButton.interactable = false;
        StartTurn();
    }

    private void SetPhase(TurnPhase phase)
    {
        Phase = phase;

        switch (Phase)
        {
            case TurnPhase.Buildings:
                StartBuildingsPhase();
                break;

            case TurnPhase.Player:
                StartPlayerPhase();
                break;
        }
    }

    private void StartTurn()
    {
        SetPhase(TurnPhase.Buildings);
    }

    private void StartBuildingsPhase()
    {
        endTurnButton.interactable = false;

        BuildingManager.Instance.ProcessTurn();
    }

    public void EndBuildingsPhase()
    {
        if (Phase != TurnPhase.Buildings)
            return;

        SetPhase(TurnPhase.Player);
    }

    private void StartPlayerPhase()
    {
        StartCoroutine(StartPlayerPhaseRoutine());
    }

    private IEnumerator StartPlayerPhaseRoutine()
    {
        IsProcessingPlayerPhase = true;
        endTurnButton.interactable = false;

        yield return BuildingManager.Instance.ProcessFires();
        yield return LineEffectManager.Instance.ProcessEffects();

        PlayerTurnData data =
            playerTurnData[Turn - 1];

        Energy = data.energy;
        MaxCardCost = data.maxCardCost;

        CardManager.Instance.DrawUpToHandSize();

        IsProcessingPlayerPhase = false;
        endTurnButton.interactable = true;
    }

    public bool SpendEnergy(int amount)
    {
        if (amount > Energy)
            return false;

        Energy -= amount;
        return true;
    }

    public void EndPlayerPhase()
    {
        if (Phase != TurnPhase.Player)
            return;

        if (GameManager.Instance.IsPlayingCard)
            return;

        EndTurn();
    }

    private void EndTurn()
    {
        if (Turn == maxTurns)
        {
            GameManager.Instance.EndGame();
        }
        else
        {
            Turn++;
            StartTurn();
        }
    }
}
