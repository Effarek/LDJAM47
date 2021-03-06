﻿using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;

public class Spaceship_Behavior : MonoBehaviour
{
    public float fuelLevel = 100f;
    public float propulsionPower = 5.0f;
    public float fuelDepletingSpeed = 20.0f;
    public float fuelReplenishingSpeed = 10.0f;
    public float additionalSpeed = 0f;
    public float soundFadout = 5.0f;
    public float rotationSpeed = 50f;

    public Image fuelBarFull;
    public GameObject orbitPoint;
    public GameObject camera;
    public Vector3 cameraOffset = new Vector3(0,0,10);
    public AudioClip thrusterGood;
    public AudioClip thrusterBad;
    public AudioClip deathSound;

    public Color currentOrbitColor;
    public Color targetOrbitColor;

    public Texture cursorImage;
    private Color cursorColor;

    private Planet_Behavior planetBehavior;
    private AudioSource thrusterSource;
    private AudioSource transferSource;

    private GameObject target;
    private ParticleSystem system;
    private Transform fuelBarParent;
    private bool overHeat = false;
    private bool IsReadyTochange = false;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        var audioSources = GetComponents<AudioSource>();
        thrusterSource = audioSources[0];
        transferSource = audioSources[1];
        system = GetComponentInChildren<ParticleSystem>();
        fuelBarParent = fuelBarFull.transform.parent;

        if (orbitPoint)
        {
            SetOrbite(orbitPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            IsReadyTochange = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            IsReadyTochange = false;
        }

        // Change orbit
        if (target && target != orbitPoint)
        {
            if (IsReadyTochange || orbitPoint == null)
            {
                SetOrbite(target);
            }

            cursorColor = targetOrbitColor;
        }
        else
        {
            cursorColor = Color.white;
        }

        if(fuelLevel > 0f)
        {
            if (Input.GetAxis("Vertical") != 0 && !overHeat)
            {
                additionalSpeed = Input.GetAxis("Vertical") * propulsionPower;
                fuelLevel -= fuelDepletingSpeed * Time.deltaTime;

                // Play thruster sound
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
                // Fadeout and stop thruster sound
                thrusterSource.volume = math.max(0, thrusterSource.volume - soundFadout * 2 * Time.deltaTime);
                if (thrusterSource.volume == 0)
                {
                    thrusterSource.Stop();
                }
                additionalSpeed = 0;
                // Stop over heat
                if (fuelLevel > 30f)
                {
                    overHeat = false;
                }
            }
        }
        else
        {
            additionalSpeed = 0;
            fuelLevel = 0;
            overHeat = true;
        }

        if(fuelLevel < 100 && (Input.GetAxis("Vertical") == 0 || overHeat))
        {
            fuelLevel += fuelReplenishingSpeed * Time.deltaTime;
        }

        // Change thruster sound
        if (fuelLevel <= 30 && thrusterSource.clip != thrusterBad){
            thrusterSource.clip = thrusterBad;
        }
        else if (fuelLevel > 30 && thrusterSource.clip != thrusterGood){
            thrusterSource.clip = thrusterGood;
        }

        if (planetBehavior)
        {
            transform.RotateAround(orbitPoint.transform.position, Vector3.back, (planetBehavior.orbitSpeed + math.sign(planetBehavior.orbitSpeed) * additionalSpeed) * Time.deltaTime);
        }

        // Rotation
        if (additionalSpeed == 0)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }

        fuelBarParent.position = Camera.main.WorldToScreenPoint(transform.position);
        var fuelratio = fuelLevel / 100f;
        fuelBarFull.fillAmount = fuelratio;
        if (overHeat)
        {
            fuelBarFull.color = new Color(1, 0, 0, 1);
        }
        else
        {
            fuelBarFull.color = new Color(1, fuelratio, fuelratio, 1);
        }

        fuelBarParent.gameObject.SetActive(fuelLevel < 100f);
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
        if (planetBehavior)
        {
            planetBehavior.isWaiting = false;
        }
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

        // Sound
        transferSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        transferSource.Play();

        IsReadyTochange = false;
    }

    public void Destroy()
    {
        var listener = GetComponentInChildren<AudioListener>().gameObject;
        listener.transform.parent = null;
        listener.GetComponent<AudioSource>().Play();
        fuelBarParent.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orbit"))
        {
            // Restore actual target
            if (target)
            {
                Circle oldTargetOrbit = target.gameObject.GetComponentInChildren<Circle>();
                if (oldTargetOrbit)
                {
                    oldTargetOrbit.SetColor(oldTargetOrbit.defaultColor);
                }
            }
            // Set new target
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

    private void OnGUI()
    {
        GUI.color = cursorColor;
        GUI.DrawTexture(new Rect(Input.mousePosition.x - 8, Screen.height - Input.mousePosition.y - 8, 16, 16), cursorImage);
    }

}
