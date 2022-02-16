using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    Camera cam;
    Transform player;
    Vector3 endLoc, lastLoc;

    float m, b;

    bool isVertical;

    void Start() {
        BulletList.addBullet(this.gameObject);
        player = GameObject.Find("Player").transform;

        cam = Camera.main;
        findEndLocation();
    }

    // Update is called once per frame
    void FixedUpdate() {
        lastLoc = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, endLoc, 6f * Time.deltaTime);
    }

    void findEndLocation() { //Find y in y = mx+b, where n is a location of screen
                             //Needed Info
        Vector3 playerPos = player.position;
        float screenHeight = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y;
        float D; //Direction Facing
        float n; //Target x
        float y; //Target y

        //Get point x
        if (transform.position.x - playerPos.x == 0) { //If Vertical Line
            Vector3 normalized = transform.position - player.position;

            if (normalized.y < 0)
                y = screenHeight * 2;
            else
                y = -screenHeight;

            //Get Line Components
                n = transform.position.x;

            isVertical = true;
            Debug.Log("Vertical Bullet");
        }
        else {
            Vector3 normalized = transform.position - player.position;

        //Get Line Components
            m = (transform.position.y - playerPos.y) / (transform.position.x - playerPos.x);
            b = transform.position.y - (m * transform.position.x);

        //Get Vector
            if (normalized.y < 0) { //Up
                if (normalized.x < 0) { //Up Right
                    n = screenHeight * 2;
                    y = (m * screenHeight * 2) + b;
                }
                else { //Up Left
                    y = screenHeight * 2;
                    n = (y - b) / m;
                }
            }
            else { //Down
                if (normalized.x < 0) { //Down Right
                    y = -screenHeight;
                    n = (y - b) / m;
                }
                else { //Down Left
                    n = -screenHeight;
                    y = (m * -screenHeight) + b;
                }
            }

            isVertical = false;
        }

        endLoc = new Vector3(n, y, transform.position.z);

        //Rotate to Face Point
        Quaternion rotateDir = Quaternion.LookRotation(Vector3.forward, -endLoc);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDir, 20000);
    }

    void OnBecameInvisible() {
        BulletList.removeBullet(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Deflector") {
            findEndLocation();
            collision.gameObject.GetComponent<Deflectors>().setToSearch();
            AudioManager.instance.Play("Missile Bounce");
        }
    }

    public float getM() {
        return m;
    }

    public float getB() {
        return b;
    }

    public bool getIsVert()
    {
        return isVertical;
    }

    public float getDistanceDetaOfPoint(Vector3 point) {
    //Last Frame Distance to point - current Distance to point
        return (Vector3.Distance(lastLoc, point) - Vector3.Distance(transform.position, point));
    }
}
