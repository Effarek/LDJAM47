using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Spaceship_Behavior player;
    public AudioClip[] musics;
    public Transform[] respawnPositions;
    public Dialogue[] dialogues;
    public Transform[] dialogueTriggers;
    public Text gameOverText;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool IsGameOver = false;
    private int currentLvl;
    private DialogueManager dialogueManager;
    private int lastDialog = 0;

    void Start()
    {
        var sources = GetComponents<AudioSource>();
        dialogueManager = FindObjectOfType<DialogueManager>();
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
        if (currentLvl == 1)
        {
            dialogueManager.StartDialogue(dialogues[0]);
        }
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

        
        if (player && player.GetComponentInParent<Planet_Behavior>())
        {
            // Update respawn points
            if (respawnPositions[currentLvl].parent == player.GetComponentInParent<Planet_Behavior>().gameObject.transform)
            {
                currentLvl += 1;
                PlayerPrefs.SetInt("lvl", currentLvl);
                if (musicSource.clip != musics[currentLvl - 1])
                {
                    musicSource.clip = musics[currentLvl - 1];
                }
            }
            // Script dialogues
            for (int i = 0; i < dialogueTriggers.Length; i++)
            {
                if(dialogueTriggers[i] == player.GetComponentInParent<Planet_Behavior>().gameObject.transform && i > lastDialog)
                {
                    lastDialog = i;
                    dialogueManager.StartDialogue(dialogues[i]);
                }
            }
            // TODO hardcode dialogues[4] + dialogues[5]
        }
    }
}
