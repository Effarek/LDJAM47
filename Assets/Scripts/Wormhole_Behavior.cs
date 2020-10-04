using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole_Behavior : MonoBehaviour
{
    public GameObject target;
    public float rotation = 50f;

    private List<GameObject> travellers;
    private ParticleSystem system;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        travellers = new List<GameObject>();

        system = GetComponent<ParticleSystem>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);

        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance < 8f && distance > 0.5f)
        {
            if (!system.isPlaying)
            {
                system.Play();
            }

            ParticleSystem.EmissionModule emissionModule = system.emission;
            emissionModule.rateOverTime = (int)(200 / distance);
        }
        else
        {
            if (system.isPlaying)
            {
                system.Stop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (target != null && !travellers.Contains(collision.gameObject))
        {
            var tar = target.GetComponent<Wormhole_Behavior>();
            if (tar)
            {
                tar.travellers.Add(collision.gameObject);
            }
            collision.transform.position = target.transform.position;

            StartCoroutine(Camera.main.GetComponent<Camera_Behaviour>().SetScreenshake());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (travellers.Contains(collision.gameObject))
        {
            travellers.Remove(collision.gameObject);
        }
    }
}
