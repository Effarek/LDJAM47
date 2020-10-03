using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite_Behaviour : MonoBehaviour
{
    public float Speed = 25;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Speed * Time.deltaTime;
    }
}
