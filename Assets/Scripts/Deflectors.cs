using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflectors : MonoBehaviour
{
    public Vector2 pos1, pos2;
    Vector3 intersectPoint;
    public float velocity;

    short state; //False = pos1    True = pos2

    float m, b;

    float timer, maxTimer = 3;

    public void setDeflectors(Vector2 startPos, Vector2 pos1, Vector2 pos2, float velocity) {
        transform.position = startPos;
        this.pos1 = pos1;
        this.pos2 = pos2;
        this.velocity = velocity;
    }

    private void Start() {
        state = 4;
        getLineInfo();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(state == 1) { //searching for bullet
            if (findBullet() == -1)
                state = 3;
            else
                state = 2;
        }
        else if(state == 2) {
            transform.position = Vector2.MoveTowards(transform.position, intersectPoint, velocity * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, intersectPoint);

            if (timer >= maxTimer) {
                state = 3;
            }else if (Vector3.Distance(transform.position, intersectPoint) < 1) {
                timer += Time.deltaTime;
            }
        }
        else if(state == 3) {
            transform.position = Vector2.MoveTowards(transform.position, pos2, velocity * Time.deltaTime);

            if(Vector3.Distance(pos2, transform.position) < 1) {
                if (findBullet() == -1)
                    state = 4;
                else
                    state = 2;
            }
        }
        else if(state == 4) {
            transform.position = Vector2.MoveTowards(transform.position, pos1, velocity * Time.deltaTime);

            if (Vector3.Distance(pos1, transform.position) < 1)
            {
                if (findBullet() == -1)
                    state = 3;
                else
                    state = 2;
            }
        }
    }

    private void Update() {
        if (Input.GetKey(KeyCode.R))
            state = 1;
    }

    void getLineInfo() { //Find ym and b using y = mx+b
        if (pos1.x != pos2.x) { 
            m = (pos2.y - pos1.y) / (pos2.x - pos1.x);
            b = transform.position.y - (m * transform.position.x);
        }
    }

    int findBullet() {
        int bestBulletIndex = -1;
        float bestDistance = 1000;
        List<GameObject> list = BulletList.bulletList;

        for (int i = 0; i < list.Count; i++) {
            BulletScript bullet = list[i].GetComponent<BulletScript>();
            float distance = checkBullet(bullet, list[i].transform.position);

            if(distance > 5/*min*/ && distance < bestDistance) {
                bestDistance = distance;
                bestBulletIndex = i;
            }
        }

        if (bestDistance == 1000)
            return -1;

        intersectPoint = getIntesect(list[bestBulletIndex].GetComponent<BulletScript>());

        return 1;
    }

    float checkBullet(BulletScript bullet, Vector3 bulletPos) {
    //If Lines are Parallel
        if ((bullet.getIsVert() && pos1.x == pos2.x && m != 0) || (m == bullet.getM() && m != 0)) {
            //print("Parallel Lines");
            return -1;
        }

        //Calculated intersect point
        Vector3 intersect = getIntesect(bullet);

        //Check for intersect off of line segment
        if ((intersect.x > pos1.x && intersect.x > pos2.x) || (intersect.x < pos1.x && intersect.x < pos2.x)) {
            //Debug.Log("x of intersect is of segment");
            return -1;
        }
        else if ((intersect.y > pos1.y && intersect.y > pos2.y) || (intersect.y < pos1.y && intersect.y < pos2.y)) {
            Debug.Log("y of intersect is of segment");
            return -1;
        }

        //Check for bullet facing away
        if (bullet.getDistanceDetaOfPoint(intersectPoint) <= 0) {
            //Debug.Log("Bullet Facing Away");
            return -1;
        }

        return Vector2.Distance(intersect, bulletPos);
    }

    Vector3 getIntesect(BulletScript bullet) {
        //Calculated intersect point
        float x;
        float y;

        if (m == bullet.getM() && m != 0)
        {
            x = transform.position.x;
            y = (bullet.getM() * x) + bullet.getB();
            //Debug.Log("IntersectPoint (Deflector Vertical)");
        }
        else
        {
            if (bullet.getIsVert())
            {
                x = bullet.transform.position.x;
                //Debug.Log("IntersectPoint (Bullet Vertical)");
            }
            else
            {
                x = (bullet.getB() - b) / (m - bullet.getM());
                //Debug.Log("IntersectPoint (Normal)");
            }

            y = (m * x) + b;
        }

        return new Vector3(x, y, transform.position.z);
    }

    public void setToSearch() {
        state = 1;
    }
}
