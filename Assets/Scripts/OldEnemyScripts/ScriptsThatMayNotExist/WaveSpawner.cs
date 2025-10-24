using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class WaveSpawner : MonoBehaviour
{//This is to show up in the Unity Inspector
    [System.Serializable]
    public class Wave 
    {public AllEnemyScript[] enemies;
     public int count;
     public float timeBetweenSpawns;
     }//the # of waves and where they can spawn at
     public Wave[] waves;
     //IMPORTANT:
     //IF I INTEND ON USING THIS SCRIPT EACH SPAWNPOINT IS AN "EMPTY" IN UNITY
     //SO I MUST CREATE EMPTIES AND NOT PUT IN THE CORDINATES MANUALLY. 
     //JUST PLACE IT WHERE I WANT THEM TO SPAWN IN AT AND PUT THEM UNDER THE WAVE SPAWNER IN A GROUP
     public Transform[] spawnPoints;
     public float timeBetweenWaves;

     private Wave currentWave;
     private int currentWaveIndex;
     private Transform player;
     
     //Note: I may bind this to an object so if I do may need to make new script for it and it will change where this gets refrenced at
     private bool finishedSpawning;

     public GameObject boss;
     public Transform bossSpawnPoint;
     public GameObject bossHealthBar;

     private void Start() {
        //finding player tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StartNextWave(currentWaveIndex));
     }//Waits a certain amount of seconds before starting next wave
     IEnumerator StartNextWave(int index){
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
      //This spawns the next wave in 
     IEnumerator SpawnWave(int index){
        currentWave = waves[index];
        for (int i = 0; i < currentWave.count; i++){
            if (player == null){
               //this breaks the code if the player is dead
                yield break;
            }
            AllEnemyScript randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation);
        //if the I variable is = to the current wave spawn minus 1 then the wave is finished spawning
            if (i == currentWave.count - 1)
            {finishedSpawning = true;}
            else{
                finishedSpawning = false;
            }
        //this makes sure the first wave spawned
            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
        }
     }
     }
     private void Update() {
        //if the wave is finished spawning and the game has no more enemies(this is the spot to change if I bind it to an object)
        //To change it to binding I should prob add if the player is touching the layers of: button && finished wave spawn is set to true
        if(finishedSpawning == true && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {finishedSpawning = false;
       //checking if a new wave exists and starting it
        if(currentWaveIndex + 1 < waves.Length)
        {currentWaveIndex++;
        StartCoroutine(StartNextWave(currentWaveIndex));}
        //All waves complete = BOSS WAVE
        else{
        Instantiate(boss, bossSpawnPoint.position, bossSpawnPoint.rotation);
        bossHealthBar.SetActive(true);
        
        }}
     }
}
*/