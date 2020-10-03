using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite_Behaviour : MonoBehaviour
{
    public float Speed = 25.0f;
    public float maxDistance = 100.0f;
    private float currentDistance = 0f;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Speed * Time.deltaTime;
        currentDistance = Vector3.Distance(player.transform.position, transform.position);

        if(currentDistance > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
