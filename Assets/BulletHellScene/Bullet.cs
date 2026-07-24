using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0f;
    public bool homing = true;
    public float lifetime = 5f;
    public float time = 0f;

    void Update() {
        //if (homing) {
        //    Vector2.Distance(BulletPlayerController.instance.transform.position, transform.position)


        //    Vector2 direction = ((Vector2)(BulletPlayerController.instance.transform.position - transform.position)).normalized;
        //    HomeBullet(direction);
        //    Debug.Log(direction.magnitude);
        //    return;
        //}
        time += Time.deltaTime;
        if (time >= lifetime) Destroy(this.gameObject);

        MoveBullet();
    }


    private void MoveBullet() {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void HomeBullet(Vector2 playerPos) {
        transform.Translate(playerPos * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Destroy(this.gameObject);
    }

}
