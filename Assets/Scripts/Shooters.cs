using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooters : MonoBehaviour {
    public Vector3 onScreen, end;

    public float startToScreenSpeed, onScreenTime, screenToEndSpeed;
    float currentTimer, shotTimer, waitTimer;
    int moveState = 0; //StartToScreen = 1, onScreen = 2, screenToEnd = 3

    public GameObject bulletTemplate;

    public void setShooters(Vector3 startPos, Vector3 onScreen, Vector3 end, float startToScreenSpeed, float onScreenTime, float screenToEndSpeed, float waitTimer) {
        transform.position = startPos;
        this.onScreen = onScreen;
        this.end = end;
        this.startToScreenSpeed = startToScreenSpeed;
        this.onScreenTime = onScreenTime;
        this.screenToEndSpeed = screenToEndSpeed;
        this.waitTimer = waitTimer;
    }


    void FixedUpdate() {
        if (moveState == 0) {
            currentTimer += Time.deltaTime;

            if (currentTimer >= waitTimer) {
                currentTimer = 0;
                moveState++;
            }

        }
        else if (moveState == 1) {
            transform.position = Vector3.MoveTowards(transform.position, onScreen, startToScreenSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, onScreen) < 0.5) {
                moveState++;
            }
        } 
        else if (moveState == 2) {
            currentTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            if (currentTimer >= onScreenTime) {
                moveState++;
            }
            if (shotTimer >= 1) {
                shootPlayer();
                shotTimer = 0;
            }
        }
        else if (moveState == 3) {
            transform.position = Vector3.MoveTowards(transform.position, end, screenToEndSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, end) < 0.5) {
                moveState++;
            }
        }
    }

    void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    void shootPlayer() {
        Object.Instantiate(bulletTemplate, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
    }
}
