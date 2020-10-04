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

    public Image fuelBarFull;
    public GameObject orbitPoint;
    public GameObject camera;
    public Vector3 cameraOffset = new Vector3(0,0,10);

    public Color currentOrbitColor;
    public Color targetOrbitColor;

    private Planet_Behavior planetBehavior;
    private AudioSource thrusterSource;
    private GameObject target;
    private ParticleSystem system;
    private Transform fuelBarParent;


    // Start is called before the first frame update
    void Start()
    {
        if (orbitPoint)
        {
            SetOrbite(orbitPoint);
        }
        thrusterSource = GetComponent<AudioSource>();
        system = GetComponentInChildren<ParticleSystem>();
        fuelBarParent = fuelBarFull.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        // Change orbit
        if (target)
        {
            if (Input.GetButton("Fire1") || orbitPoint == null)
            {
                SetOrbite(target);
            }
        }

        if(fuelLevel > 0f)
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
                // Play fire particles
                if (!system.isPlaying)
                {
                    system.Play();
                }
            }
            else
            {
                // Stop fire particles
                if (system.isPlaying)
                {
                    system.Stop();
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
            thrusterSource.volume = math.max(0, thrusterSource.volume - soundFadout * 2 * Time.deltaTime);
            if (thrusterSource.volume == 0)
            {
                thrusterSource.Stop();
            }
        }

        if (planetBehavior)
        {
            transform.RotateAround(orbitPoint.transform.position, Vector3.back, (planetBehavior.orbitSpeed + additionalSpeed) * Time.deltaTime);
        }

        fuelBarParent.position = Camera.main.WorldToScreenPoint(transform.position);
        fuelBarParent.eulerAngles = transform.eulerAngles;
        fuelBarFull.fillAmount = fuelLevel / 100f;
    }

    void SetOrbite(GameObject newPlanet)
    {
        // Set old orbit color
        if (orbitPoint)
        {
            Circle oldOrbit = orbitPoint.GetComponentInChildren<Circle>();
            if (oldOrbit)
            {
                oldOrbit.SetColor(oldOrbit.defaultColor);
            }
        }

        // Move to next orbit
        orbitPoint = newPlanet;
        planetBehavior = orbitPoint.GetComponent<Planet_Behavior>();
        transform.parent = orbitPoint.transform;

        Circle newOrbit = orbitPoint.GetComponentInChildren<Circle>();
        if (newOrbit)
        {
            // Update orbits color
            newOrbit.SetColor(currentOrbitColor);
            // Set the position on the orbite
            var normPos = new Vector2(transform.localPosition.x, transform.localPosition.y).normalized;
            transform.localPosition = new Vector3(
                normPos.x * newOrbit.xRadius * newOrbit.gameObject.transform.localScale.x,
                normPos.y * newOrbit.yRadius * newOrbit.gameObject.transform.localScale.y,
                transform.localPosition.z
            );
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
