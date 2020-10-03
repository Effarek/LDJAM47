using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Behavior : MonoBehaviour
{
    public float orbitSpeed = 30f;
    public float fuelLevel = 100f;
    public float PropulsionPower = 5.0f;
    public float fuelDepletingSpeed = 20.0f;
    public float fuelReplenishingSpeed = 10.0f;

    public GameObject OrbitPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && fuelLevel > 0)
        {
            transform.RotateAround(OrbitPoint.transform.position, Vector3.back, orbitSpeed * PropulsionPower * Time.deltaTime);
            fuelLevel -= fuelDepletingSpeed * Time.deltaTime;
        }
        else
        {
            transform.RotateAround(OrbitPoint.transform.position, Vector3.back, orbitSpeed * Time.deltaTime);
            if (fuelLevel < 100 && !Input.GetKey(KeyCode.Space))
            {
                fuelLevel += fuelReplenishingSpeed * Time.deltaTime;
            }          
        }
        
    }
}
