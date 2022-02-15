using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public Wave[] waves;

    float timer = 0, currentWaveTime;
    int currentWave = 0;


    private void Awake() {
        currentWaveTime = waves[0].waveTime;
        waves[currentWave].spawnEnemies();
        print("wave");
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentWaveTime) {
            Debug.LogError("Wave " + (currentWave+1) + " Finshed");
            timer = 0;

            currentWave++;

            if (currentWave > waves.Length)
                win();

            currentWaveTime = waves[currentWave].waveTime;
            waves[currentWave].spawnEnemies();
        }
    }

    private void win() {
        this.enabled = false;
        print("Win");
    }
}
