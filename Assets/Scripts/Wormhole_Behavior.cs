using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Wormhole_Behavior : MonoBehaviour
{
    public GameObject target;
    public GameObject orbitPoint;
    public float rotation = 50f;
    public bool singleTicket = false;
    public bool shouldShake = true;
    public bool inverteRotation = false;

    private List<GameObject> travellers;
    private ParticleSystem system;
    private GameObject player;
    private bool isDying = false;

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
        
        float distance;
        
        if (player)
        {
            distance = Vector2.Distance(player.transform.position, transform.position);
        }
        // Très sale nounours
        else
        {
            distance = 1000;
        }
        

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

        if (isDying)
        {
            var newScale = math.max(0, transform.localScale.x - 0.1f * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
            if(newScale == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null && !travellers.Contains(collision.gameObject) && !isDying)
        {
            Camera_Behaviour cam = Camera.main.GetComponent<Camera_Behaviour>();
            Vector3 pPos;
            if (cam.player)
            {
                pPos = cam.player.transform.position;
            }
            else
            {
                pPos = cam.transform.position;
            }
            // Teleport
            collision.transform.position = target.transform.position;
            var tar = target.GetComponent<Wormhole_Behavior>();
            // Has player teleported
            bool playerTP = (cam.player != null && pPos != cam.player.transform.position);

            // Other side is wormhole
            if (tar)
            {
                tar.travellers.Add(collision.gameObject);
                var planet = collision.gameObject.GetComponent<Planet_Behavior>();
                if (planet)
                {
                    // Set new orbit
                    planet.orbitPoint = tar.orbitPoint;
                    if (inverteRotation)
                    {
                        planet.orbitSpeed = -planet.orbitSpeed;
                        planet.rotationSpeed = -planet.rotationSpeed;
                    }
                }
            }

            if (playerTP)
            {
                if (shouldShake)
                {
                    StartCoroutine(cam.SetScreenshake());
                }

                if (singleTicket || (tar && tar.singleTicket))
                {
                    Disappear();
                    if (tar)
                    {
                        tar.Disappear();
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (travellers.Contains(collision.gameObject))
        {
            travellers.Remove(collision.gameObject);
        }
    }

    public void Disappear()
    {
        isDying = true;
    }
}
