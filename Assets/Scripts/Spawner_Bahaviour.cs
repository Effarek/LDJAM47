﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Bahaviour : MonoBehaviour
{
    public float timerDuration = 5.0f;
    private float timer = 0.0f;
    private bool spanwed = false;

    public GameObject objectToSpawn;
    public float spawnedObjectMaxDistance = 100;


    // Update is called once per frame
    void Update()
    {
        if(timer < timerDuration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject meteoriteCreated = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            meteoriteCreated.transform.rotation = transform.rotation;
            meteoriteCreated.GetComponent<Meteorite_Behaviour>().maxDistance = spawnedObjectMaxDistance;
            timer = 0;
        }
    }
}
