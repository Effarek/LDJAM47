using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;
using JetBrains.Annotations;

public class Spaceship_Behavior : MonoBehaviour
{
    public float fuelLevel = 100f;
    public float propulsionPower = 5.0f;
    public float fuelDepletingSpeed = 20.0f;
    public float fuelReplenishingSpeed = 10.0f;
    public float additionalSpeed = 0f;
    public float soundFadout = 5.0f;

    public GameObject orbitPoint;
    public GameObject camera;
    public Vector3 cameraOffset = new Vector3(0,0,10);

    public Color currentOrbitColor;
    public Color targetOrbitColor;

    private Planet_Behavior planetBehavior;
    private AudioSource thrusterSource;
    private GameObject target;
    

    // Start is called before the first frame update
    void Start()
    {
        if (orbitPoint)
        {
            SetOrbite(orbitPoint);
        }
        thrusterSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(orbitPoint);
        // Change orbit
        if (target)
        {
            if (Input.GetButton("Fire1") || orbitPoint == null)
            {
                SetOrbite(target);
            }
        }

        if(fuelLevel > 0)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                additionalSpeed = Input.GetAxis("Vertical") * propulsionPower;
                fuelLevel -= fuelDepletingSpeed * Time.deltaTime;
                // Thruster sound
                thrusterSource.volume = math.min(1, thrusterSource.volume + soundFadout * Time.deltaTime);
                if (!thrusterSource.isPlaying)
                {
                    thrusterSource.Play();
                }
            }
        }
        else
        {
            additionalSpeed = 0;
            fuelLevel = 0;
        }

        if(fuelLevel < 100 && Input.GetAxis("Vertical") == 0)
        {
            fuelLevel += fuelReplenishingSpeed * Time.deltaTime;
        }

        // Fadout sound
        if (Input.GetAxis("Vertical") == 0)
        {
            thrusterSource.volume = math.max(0, thrusterSource.volume - soundFadout * Time.deltaTime);
            if (thrusterSource.volume == 0)
            {
                thrusterSource.Stop();
            }
        }

        if (planetBehavior)
        {
            transform.RotateAround(orbitPoint.transform.position, Vector3.back, (planetBehavior.orbitSpeed + additionalSpeed) * Time.deltaTime);
        }
    }

    void SetOrbite(GameObject newPlanet)
    {
        // Set old orbit color
        Circle oldOrbit = orbitPoint.GetComponentInChildren<Circle>();
        if (oldOrbit)
        {
            oldOrbit.SetColor(oldOrbit.defaultColor);
        }

        // Move to next orbit
        orbitPoint = newPlanet;
        planetBehavior = orbitPoint.GetComponent<Planet_Behavior>();
        transform.parent = orbitPoint.transform;

        // Update orbits color
        Circle newOrbit = orbitPoint.GetComponentInChildren<Circle>();
        if (newOrbit)
        {
            newOrbit.SetColor(currentOrbitColor);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orbit"))
        {
            target = collision.gameObject.transform.parent.gameObject;
            Circle targetOrbit = collision.gameObject.GetComponent<Circle>();
            if (targetOrbit)
            {
                targetOrbit.SetColor(targetOrbitColor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.gameObject == target)
        {
            Circle targetOrbit = collision.gameObject.GetComponent<Circle>();
            if (targetOrbit)
            {
                targetOrbit.SetColor(targetOrbit.defaultColor);
            }
            target = null;
        }
    }
}
