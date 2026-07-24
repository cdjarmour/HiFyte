using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Battle;
using TMPro;
using UnityEngine;

public class BattleText : MonoBehaviour {
    [SerializeField] TMP_Text playerAttackText;
    [SerializeField] TMP_Text comboTally;
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text playerHp;
    [SerializeField] TMP_Text enemyHp;



    private void OnEnable() {
        ActionManager.OnPlayerAttack += OnPlayerAttack;
        Battle.BeatManager.OnCountdown += AttackStartup;
        Enemy.OnHealthChange += OnEnemyHealthChange; 
    }


    public void AttackStartup(int timerPos) {
        int count = timerPos / 2;


        if (count == 4)
            timer.gameObject.SetActive(true);

        timer.text = count.ToString();

        if (count == 0)
            timer.gameObject.SetActive(false);
    }


    public void OnEnemyHealthChange(int hp) {
        enemyHp.text = hp.ToString();
    }


    public void OnPlayerAttack(AttackAction attack) {
        playerAttackText.text = attack.movename;
    }

    private void Update() {
        comboTally.text = BattleData.instance.getCombo().ToString();
    }
}
