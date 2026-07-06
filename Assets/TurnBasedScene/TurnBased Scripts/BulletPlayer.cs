using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject parentBullet;

    private void Start() {
        BattleManager.OnBattleStateChange += OnEnemyTurn;
        parentBullet.SetActive(false);
    }


    void Update() {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0f) * speed * Time.deltaTime;
        transform.position += movement;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        Destroy(collision.gameObject);
    }



    private void OnEnemyTurn(BattleState state) {

        switch (state) {
            case BattleState.EnemyTurn:
            parentBullet.SetActive(true); break;
            default:
            parentBullet.SetActive(false);
            break;
        }


    } 
}