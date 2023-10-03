using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //variables declaretion
    [SerializeField] int MonstersAtTheMonitor;
    [SerializeField] GameObject MonsterPrefab;
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] int SpawnRange;
    private float Timer;
    void Update()
    {
        //simple timer
        Timer += Time.deltaTime;

        //check if the timer bigger then spawn range and if it's not enough monsters
        if (Timer >= SpawnRange && GameObject.FindGameObjectsWithTag("Monster").Length < MonstersAtTheMonitor)
        {
            //choose randome spawn point
            Transform randomPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

            // spawn monster
            Instantiate(MonsterPrefab, randomPoint.position, Quaternion.identity);

            //reset timer
            Timer = 0;
        }
    }
}

