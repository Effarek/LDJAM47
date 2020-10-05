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

        Circle newOrbit = orbitPoint.GetComponentInChildren<Circle>();
        if (newOrbit)
        {
            // Set the position on the orbite
            var normPos = new Vector2(transform.localPosition.x, transform.localPosition.y).normalized;
            transform.localPosition = new Vector3(
                normPos.x * newOrbit.xRadius * newOrbit.gameObject.transform.localScale.x,
                normPos.y * newOrbit.yRadius * newOrbit.gameObject.transform.localScale.y,
                transform.localPosition.z
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orbit"))
        {
            SetOrbite(collision.gameObject.transform.parent.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Spaceship_Behavior>().Destroy();
        }
    }
}
