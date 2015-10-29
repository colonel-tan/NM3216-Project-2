﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MeteroidManager : MonoBehaviour {
   
    public GameObject[] spawnPoints;
    public GameObject[] warningPoints;

    public GameObject blackHole;
    public GameObject player;
    public GameObject meteroids;
    public GameObject warningSign;

    public List<GameObject> spawnedMeteroids;

    public float minTimeBetweenSpawn = 0.5f;
    public float minTimeFirstSpawn = 15.0f;
    public float maxSpawnTime = 60.0f;
    public float finalMaxSpawnTime = 30.0f;
    public float maxSpawnTimeDecrement = 2.5f;
    public float decrementDelay = 15.0f;
    public float warningSignDuration = 2.0f;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Spawn", (float)(Random.Range(minTimeFirstSpawn, maxSpawnTime)), (float)(Random.Range(minTimeBetweenSpawn, maxSpawnTime)));
        
        player = GameObject.FindGameObjectWithTag("Player");
        meteroids = GameObject.Find("meteroid");
        warningSign = GameObject.Find("meteroidWarning");
        spawnPoints = GameObject.FindGameObjectsWithTag("Meteroid Spawner");
        warningPoints = GameObject.FindGameObjectsWithTag("Warning Spawner");
        blackHole = GameObject.FindGameObjectWithTag("Black Hole");

        spawnPoints = spawnPoints.OrderBy(go => go.name).ToArray();
        warningPoints = warningPoints.OrderBy(go => go.name).ToArray();
    }

    // Spawn meteroids
    void Spawn()
    {

        if (player == null) //If player dies
            return;

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        GameObject meteroid = GameObject.Instantiate(meteroids, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation) as GameObject;

        GameObject warning = GameObject.Instantiate(warningSign, warningPoints[spawnPointIndex].transform.position, warningPoints[spawnPointIndex].transform.rotation) as GameObject;
        Destroy(warning, warningSignDuration);

        Vector3 direction = blackHole.transform.position - meteroid.transform.position;
        direction = direction.normalized;
        meteroid.GetComponent<Rigidbody2D>().AddForce(direction);

        MeteroidManager MM = (MeteroidManager)GameObject.Find("MeteroidManager").GetComponent("MeteroidManager");
        MM.spawnedMeteroids.Add(meteroid);

    }

    void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad % decrementDelay == 0.0f && Time.timeSinceLevelLoad != 0.0f && maxSpawnTime > finalMaxSpawnTime)
        {
            maxSpawnTime -= maxSpawnTimeDecrement;
        }
    }
}
