using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0, 0, 10);
    public GameObject Player;
    public Vector3 targetPosition;

    public float lerpSpeed = 1.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = Player.transform.parent.position - cameraOffset;
        float step = lerpSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if(Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            transform.position = targetPosition;
        }
        //transform.position = Player.transform.parent.position - cameraOffset;
    }
}
