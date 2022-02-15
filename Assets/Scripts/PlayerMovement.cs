using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    Vector3 newLoc;
    public float speed = 5f;
    Camera cam;

    Animator animator;

    int currentHealth;
    public int maxHealth = 100;

    bool attacking;
    int attacks, maxAttacks;
    int rechargeTimer = 0;


    Vector2 topRightCorner, bottomLeftCorner;

    public ProgressBar healthBar;

    void Start() {
        cam = Camera.main;
        topRightCorner = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        bottomLeftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        topRightCorner = new Vector2(topRightCorner.x - 2f, topRightCorner.y - 2f);
        bottomLeftCorner = new Vector2(bottomLeftCorner.x + 2f, bottomLeftCorner.y + 2f);

        newLoc = transform.position;
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        playerDamaged(0);
    }

    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, newLoc, speed * Time.deltaTime);

        if (transform.position != Vector3.MoveTowards(transform.position, newLoc, speed * Time.deltaTime)) {
            animator.SetFloat("Speed", speed * Time.deltaTime);
        } else
            animator.SetFloat("Speed", 0);

        Vector3 moveVec = newLoc - transform.position;
        if (moveVec != Vector3.zero) {
            Quaternion rotateDir = Quaternion.LookRotation(Vector3.forward, moveVec);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDir, 500 * Time.deltaTime);
        }

        if (currentHealth <= 0) {
            print("Dead");
        }

        //attacking
        if (attacking && Vector3.Distance(newLoc, transform.position) < 1) {
            attacking = false;
        }


    //Attack recharge
    }

    void Update() {

        if (Input.GetMouseButton(0) && !attacking) { 
            float z = transform.position.z;
            newLoc = cam.ScreenToWorldPoint(Input.mousePosition);
            newLoc = new Vector3(Mathf.Clamp(newLoc.x, bottomLeftCorner.x, topRightCorner.x), Mathf.Clamp(newLoc.y, bottomLeftCorner.y, topRightCorner.y), z);
        }
        if (Input.GetMouseButton(1) && !attacking) {
            attacking = true;

            float z = transform.position.z;
            newLoc = cam.ScreenToWorldPoint(Input.mousePosition);
            newLoc = new Vector3(Mathf.Clamp(newLoc.x, bottomLeftCorner.x, topRightCorner.x), Mathf.Clamp(newLoc.y, bottomLeftCorner.y, topRightCorner.y), z);
        }
    }

    void playerDamaged(int damage) {
        currentHealth -= damage;
        healthBar.BarValue = ((float)currentHealth/maxHealth) * 100;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        string tag = collision.gameObject.tag;

        print(tag);
        if (tag == "Missile")
        {
            playerDamaged(2);
        }
        else if (tag == "Shooter" || tag == "Deflector")
        {
            playerDamaged(1);
        }
    }
}
