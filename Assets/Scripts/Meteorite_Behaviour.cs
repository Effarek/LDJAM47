using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite_Behaviour : MonoBehaviour
{
    public float speed = 25.0f;
    public float maxDistance = 200.0f;
    private float currentDistance = 0f;
    public GameObject player;

    private float lifeDuration = 15f;
    private float timeAlive;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        transform.position += transform.right * speed * Time.deltaTime;
        if (player)
        {
            currentDistance = Vector3.Distance(player.transform.position, transform.position);
        }

        if (currentDistance > maxDistance)
        {
            Destroy(gameObject);
        }

        if (timeAlive > lifeDuration)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Player"))
                {
                    child.GetComponent<Spaceship_Behavior>().Destroy();
                    break;
                }
            }

            Destroy(gameObject);
        }
    }
}
