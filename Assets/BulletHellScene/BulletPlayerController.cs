using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayerController : MonoBehaviour
{
    private const int SPEED = 5;
    float dirX = 0;
    float dirY = 1.8f;
    float moveX;
    float moveY;

    public static BulletPlayerController instance { get; private set; }

    private void Start() {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) dirX = 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) dirX = -1;

        if (Input.GetKeyDown(KeyCode.UpArrow)) dirY = Mathf.Clamp(transform.localPosition.y + 0.4f, 1f, 2.6f);
        

        if (Input.GetKeyDown(KeyCode.DownArrow)) dirY = Mathf.Clamp(transform.localPosition.y - 0.4f, 1f, 2.6f);

        moveY = Mathf.MoveTowards(transform.localPosition.y, dirY, SPEED * Time.deltaTime);
        moveX = transform.localPosition.x + dirX * SPEED * Time.deltaTime;

        transform.localPosition = new Vector2(moveX, moveY);




    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("BounceWall")) return;
        Debug.Log("huh");
        dirX = collision.contacts[0].normal.x;

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        
    }

}
