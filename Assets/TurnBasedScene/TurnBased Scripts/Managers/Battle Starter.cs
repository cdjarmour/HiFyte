using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    #region Managers
    [SerializeField] List<GameObject> initialSystems;
    [SerializeField] Battle.Player player;
    #endregion Managers


    [SerializeField] private AttackAction[] actions = new AttackAction[4]; 

    private void Awake() {

        player.setAttacks(actions);
        player.gameObject.SetActive(true);

        foreach (GameObject go in initialSystems) {
            go.SetActive(true);
        }

    }


}
