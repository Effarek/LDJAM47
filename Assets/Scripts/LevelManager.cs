using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Spaceship_Behavior player;
    public AudioClip[] musics;
    public Transform[] respawnPositions;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("lvl", 1);
    }

    void Update()
    {
        
    }
}
