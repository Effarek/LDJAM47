using System.Collections;
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
    private bool isScreenshaking = false;
    private float timeElapsed = 0f;

    private float minSize = 5f;
    private float maxSize = 50f;
    private float sensitivity = 10f;

    private void Start()
    {
        if (player.transform.parent)
        {
            playerParent = player.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }
        if (player.transform.parent)
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
        }
        else
        {
            targetPosition = new Vector3(
                Mathf.Clamp(player.transform.position.x, -maxX, maxX),
                Mathf.Clamp(player.transform.position.y, -maxY, maxY),
                -cameraOffset);
        }

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

        if (isScreenshaking)
        {
            float shakeSpeed = 80f;
            float shakeStrength = 0.2f;

            Vector3 originalPosition = transform.position;

            timeElapsed += Time.deltaTime;

            if ((int)(timeElapsed * 100) % 2 == 0)
            {
                transform.position = originalPosition
                                    + new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeStrength,
                                                  Mathf.Sin(Time.time * shakeSpeed) * shakeStrength,
                                                  0);
            }
            else
            {
                transform.position = originalPosition
                                    + new Vector3(Mathf.Sin(Time.time * shakeSpeed) * -shakeStrength,
                                                  Mathf.Sin(Time.time * shakeSpeed) * shakeStrength,
                                                  0);
            }
        }

        float size = Camera.main.orthographicSize;
        size -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minSize, maxSize);
        Camera.main.orthographicSize = size;
    }

    public IEnumerator SetScreenshake()
    {
        isScreenshaking = true;

        yield return new WaitForSeconds(0.5f);

        isScreenshaking = false;

        timeElapsed = 0f;
    }
}
