using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private const BattleState STARTING_STATE = BattleState.PlayerTurn;
    private BattleState currentState;
    private BattleState previousState;

    public static BattleManager instance {  get; private set; }

    public static event Action<BattleState> OnBattleStateChange;

    public void Awake() {
        instance = this;
    }

    public void OnEnable() {
        Battle.BeatManager.OnTimerExpired += OnTimerExpire;
    }

    public void Start() {
        UpdateBattleState(STARTING_STATE);
    }


    public void UpdateBattleState(BattleState state) {
        previousState = currentState;
        currentState = state;

        Debug.Log(state);
        

        OnBattleStateChange?.Invoke(state);

    }

    public void OnStartUpFinish() {
        Battle.BeatManager.OnCountDownEnd -= OnStartUpFinish;
        OnBattleStateChange?.Invoke(currentState);
        Debug.Log("start");
    } 


    public void OnTimerExpire() {
        switch (currentState) {
            case BattleState.PlayerTurn:
            UpdateBattleState(BattleState.EnemyTurn);
            break;
            case BattleState.PlayerAttack:
            UpdateBattleState(BattleState.EndTurn);
            break;
            case BattleState.EndTurn:
            UpdateBattleState(BattleState.EnemyTurn);
            break;
            case BattleState.EnemyTurn:
            UpdateBattleState(BattleState.PlayerTurn);
            break;
        }
    }
}


public enum BattleState {
    PlayerTurn,
    PlayerAttack,
    EnemyTurn,
    EnemyAttack,
    EndTurn,
    Victory,
    Loss
}