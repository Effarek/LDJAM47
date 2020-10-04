using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite_Behaviour : MonoBehaviour
{
    public float speed = 25.0f;
    public float maxDistance = 200.0f;
    private float currentDistance = 0f;
    public GameObject LevelCenter;

    void Start()
    {
        LevelCenter = GameObject.FindGameObjectWithTag("LevelCenter");   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        currentDistance = Vector3.Distance(LevelCenter.transform.position, transform.position);

        if (currentDistance > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
