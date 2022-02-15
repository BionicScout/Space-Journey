using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletList {
    public static List<GameObject> bulletList = new List<GameObject>();

    public static void addBullet(GameObject newBullet) {
        bulletList.Add(newBullet);
    }

    public static void removeBullet(GameObject oldBullet) {
        bulletList.Remove(oldBullet);
    }
}
