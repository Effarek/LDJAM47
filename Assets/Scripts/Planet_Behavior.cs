using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Behavior : MonoBehaviour
{
    public GameObject orbitPoint;

    public float rotationSpeed;
    public float orbitSpeed;
    public bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var finalSpeed = isWaiting ? 0 : rotationSpeed;
        transform.RotateAround(orbitPoint.transform.position, Vector3.forward, finalSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
