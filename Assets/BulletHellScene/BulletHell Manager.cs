using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{

    [SerializeField] private GameObject assets;
    private void OnEnable() {
        BattleManager.OnBattleStateChange += OnBulletStart;
    }



    private void OnBulletStart(BattleState state) {
        if (state != BattleState.EnemyTurn) {
            assets.SetActive(false);
            return;
        }
        assets.SetActive(true);
    }
}
