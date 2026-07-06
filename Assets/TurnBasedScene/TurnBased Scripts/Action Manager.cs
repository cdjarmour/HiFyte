using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> actionButtons;
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    public static event Action<AttackAction> OnPlayerAttack;

    private bool selecting = false;
    private int count = 0;


    private void OnEnable() {
        BattleManager.OnBattleStateChange += EnablePlayerUI;
    }

    private void OnDisable() {
        BattleManager.OnBattleStateChange -= EnablePlayerUI;
    }


    private void Update() {
        if (!selecting) return;

        if (Input.GetKeyDown(KeyCode.Return)) {
            BattleManager.instance.UpdateBattleState(BattleState.PlayerAttack);
            OnPlayerAttack?.Invoke(player.getAction(count));

        }



        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            count = (count + 1) % 4;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            count = (count - 1 + 4) % 4;
        }


        //Color Change
        for (int i = 0; i < actionButtons.Count; i++) {
            if (i == count) {
                actionButtons[i].GetComponent<Image>().color = Color.green;
            } else {
                actionButtons[i].GetComponent<Image>().color = Color.white;
            }
        }

    }




    public void Start() {
        AttackAction[] actions;
        actions = player.getMoveList();
        for (int i = 0; i < actionButtons.Count; i++) {
            actionButtons[i].GetComponentInChildren<TMP_Text>().text = actions[i].movename;
            int index = i;
        }
    }


    public void EnablePlayerUI(BattleState state) {
        foreach (GameObject b in actionButtons) {
            b.gameObject.SetActive(state == BattleState.PlayerTurn);
        }
        selecting = state == BattleState.PlayerTurn;
    }
}

