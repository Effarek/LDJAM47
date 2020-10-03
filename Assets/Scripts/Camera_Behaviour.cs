using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0, 0, 10);
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.parent.position - cameraOffset;
    }
}
