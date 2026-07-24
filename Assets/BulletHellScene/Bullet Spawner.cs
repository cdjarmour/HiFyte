using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float burstCount;
    [SerializeField][Range(0f, 359f)] float angleSpread;
    [SerializeField] int totalBullets;
    private bool isShooting = false;


    List<GameObject> bullets = new List<GameObject>();

    private float timePerShot;

    private void OnEnable() {
       timePerShot = Battle.BeatManager.instance.getBeatLength() * 4f;
    }

    private void OnDisable() {
        isShooting = false;
        removeBullets();
        StopAllCoroutines();
    }


    private void Update() {
        if (!isShooting) {
            StartCoroutine(bulletSpawner());
        }


    }

    public IEnumerator bulletSpawner() {
        isShooting = true;

        //Temporary
        angleSpread = (int) Random.Range(0, 359);
        totalBullets = Random.Range(3, 9);
        Vector2 targetNormal;
        int random = Random.Range(0, 2);
        if (random == 0) {
            targetNormal = new Vector2(0, -1);
        } else {
            targetNormal = BulletPlayerController.instance.transform.position - transform.position;
        }

        //Temporary

        //Vector2 targetNormal = new Vector2(0, -1);
        float targetAngle = Mathf.Atan2(targetNormal.y, targetNormal.x) * Mathf.Rad2Deg;

        float angleStep = angleSpread / (totalBullets - 1);
        float halfAngleSpread = angleSpread / 2f;
        float startAngle = targetAngle - halfAngleSpread;
        float endAngle = targetAngle + halfAngleSpread;
        float currentAngle = startAngle;

        for (int i = 0; i < totalBullets; i++) { 
            Vector2 pos = bulletSpreadAngles(currentAngle);
            GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
            bullets.Add(bullet);
            bullet.transform.right = (Vector2) (bullet.transform.position - transform.position).normalized;
            bullet.GetComponent<Bullet>().speed = bulletSpeed;
            currentAngle += angleStep;
        }

        yield return new WaitForSeconds(timePerShot);
        isShooting = false;
    }


    private Vector2 bulletSpreadAngles(float angle) {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) + transform.position.x;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) + transform.position.y;

        return new Vector2(x, y);
    }

    public void removeBullets() {
        foreach (GameObject bullet in bullets) {
            Destroy(bullet);
        }
        bullets.Clear();
    }

}
