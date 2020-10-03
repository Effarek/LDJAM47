using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien_Behavior : MonoBehaviour
{
    public GameObject orbitPoint;

    private Planet_Behavior planetBehavior;

    // Start is called before the first frame update
    void Start()
    {
        if (orbitPoint)
        {
            SetOrbite(orbitPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (planetBehavior)
        {
            transform.RotateAround(orbitPoint.transform.position, Vector3.back, planetBehavior.orbitSpeed * Time.deltaTime);
        }
    }

    void SetOrbite(GameObject newPlanet)
    {
        orbitPoint = newPlanet;
        planetBehavior = orbitPoint.GetComponent<Planet_Behavior>();
        transform.parent = orbitPoint.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orbit"))
        {
            SetOrbite(collision.gameObject.transform.parent.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
