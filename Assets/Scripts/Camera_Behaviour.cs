﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{
    public GameObject player;
    public float maxX = 70.0f;
    public float maxY = 70.0f;
    public float cameraOffset = 10;

    private Vector3 targetPosition;
    private float currentTime = 0;
    public float lerpDuration = 1.0f;

    private GameObject playerParent;
    private bool playerParentChanged = false;

    private void Start()
    {
        playerParent = player.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerParent != player.transform.parent.gameObject)
        {
            playerParentChanged = true;
            playerParent = player.transform.parent.gameObject;
        }

        targetPosition = new Vector3(
            Mathf.Clamp(player.transform.parent.position.x, -maxX, maxX), 
            Mathf.Clamp(player.transform.parent.position.y, -maxY, maxY), 
            -cameraOffset);

        if (playerParentChanged)
        {
            if (currentTime < lerpDuration)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, currentTime / lerpDuration);
                currentTime += Time.deltaTime;
            }
            else
            {
                playerParentChanged = false;
            }
        }
        else
        {
            transform.position = targetPosition;
        }
        
        
    }
}
