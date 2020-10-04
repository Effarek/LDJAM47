using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite_Behaviour : MonoBehaviour
{
    public float speed = 25.0f;
    public float maxDistance = 200.0f;
    private float currentDistance = 0f;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        currentDistance = Vector3.Distance(player.transform.position, transform.position);

        if (currentDistance > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
