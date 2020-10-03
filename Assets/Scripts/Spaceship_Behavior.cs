using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Behavior : MonoBehaviour
{
    public float fuelLevel = 100f;
    public float propulsionPower = 5.0f;
    public float fuelDepletingSpeed = 20.0f;
    public float fuelReplenishingSpeed = 10.0f;

    public GameObject orbitPoint;

    private Planet_Behavior planetBehavior;

    // Start is called before the first frame update
    void Start()
    {
        SetOrbite(orbitPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && fuelLevel > 0)
        {
            transform.RotateAround(orbitPoint.transform.position, Vector3.back, planetBehavior.orbitSpeed * propulsionPower * Time.deltaTime);
            fuelLevel -= fuelDepletingSpeed * Time.deltaTime;
        }
        else
        {
            transform.RotateAround(orbitPoint.transform.position, Vector3.back, planetBehavior.orbitSpeed * Time.deltaTime);
            if (fuelLevel < 100 && !Input.GetKey(KeyCode.Space))
            {
                fuelLevel += fuelReplenishingSpeed * Time.deltaTime;
            }          
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
        if (collision.gameObject.CompareTag("Planet"))
        {
            SetOrbite(collision.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
