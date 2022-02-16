using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public float waveTime;

    public ShooterInfo[] shooters;
    public DeflectorInfo[] deflectors;

    public GameObject shooterPrefab, deflectorPrefab;

    public void spawnEnemies() {
        GameObject temp;

        //Spawn Shooters
        for(int i = 0; i < shooters.Length; i++) {
            temp = GameObject.Instantiate(shooterPrefab);
            shooters[i].getScript(temp);
        }

        //Spawn Deflectors
        for (int i = 0; i < deflectors.Length; i++)
        {
            temp = GameObject.Instantiate(deflectorPrefab);
            deflectors[i].getScript(temp);
        }
    }


}
