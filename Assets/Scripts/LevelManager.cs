using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Spaceship_Behavior player;
    public AudioClip[] musics;
    public Transform[] respawnPositions;
    public Dialogue[] dialogues;
    public Text gameOverText;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool IsGameOver = false;
    private int currentLvl;
    // TODO dialogues
    // TODO respawn points


    void Start()
    {
        var sources = GetComponents<AudioSource>();
        musicSource = sources[0];
        currentLvl = PlayerPrefs.GetInt("lvl", 1);
        // Set spawn position
        player.transform.position = new Vector3(
            respawnPositions[currentLvl - 1].position.x,
            respawnPositions[currentLvl - 1].position.y,
            player.transform.position.z
        );
        // Load level music
        musicSource.clip = musics[currentLvl - 1];
        musicSource.Play();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues[0]);
    }

    void Update()
    {
        if (!IsGameOver && player == null)
        {
            IsGameOver = true;
            gameOverText.enabled = true;
        }

        if (IsGameOver && Input.anyKeyDown)
        {
            SceneManager.LoadScene("FullLevels", LoadSceneMode.Single);
        }

        // TODO respawn points

        // TODO remove wormholes on respawn

        // TODO dialogues
    }
}
