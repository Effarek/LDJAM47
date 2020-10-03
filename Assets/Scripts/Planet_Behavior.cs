using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Behavior : MonoBehaviour
{
    public GameObject OrbitPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(OrbitPoint.transform.position, Vector3.forward, 15 * Time.deltaTime);
    }
}
