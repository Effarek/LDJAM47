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
    public Text pauseText;
    public Transform lastSun;

    private AudioSource musicSource;
    private bool IsGameOver = false;
    private int currentLvl;
    private DialogueManager dialogueManager;
    private int lastDialog = 0;
    private float lvlTimer = 0.0f;

    void Start()
    {
        var sources = GetComponents<AudioSource>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        musicSource = sources[0];
        //PlayerPrefs.SetInt("lvl", 6);
        currentLvl = PlayerPrefs.GetInt("lvl", 1);
        // Set spawn position
        player.transform.position = new Vector3(
            respawnPositions[currentLvl - 1].position.x,
            respawnPositions[currentLvl - 1].position.y,
            player.transform.position.z
        );
        // Load level music
        if (musics.Length >= currentLvl)
        {
            musicSource.clip = musics[currentLvl - 1];
            musicSource.Play();
        }

        if (currentLvl == 1)
        {
            dialogueManager.StartDialogue(dialogues[0]);
        }
    }

    void Update()
    {
        // Back to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseText.enabled)
            {
                pauseText.enabled = false;
                Time.timeScale = 1;
            }
            else
            {
                pauseText.enabled = true;
                Time.timeScale = 0;
            }
        }

        if (pauseText.enabled && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("MainMenu");
        }

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
            if (currentLvl <= respawnPositions.Length - 1)
            {
                // Update respawn points
                if (respawnPositions[currentLvl].parent == player.GetComponentInParent<Planet_Behavior>().gameObject.transform)
                {
                    currentLvl += 1;
                    lvlTimer = Time.time;
                    PlayerPrefs.SetInt("lvl", currentLvl);
                    if (musicSource.clip != musics[currentLvl - 1])
                    {
                        musicSource.clip = musics[currentLvl - 1];
                        musicSource.Play();
                    }
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
            if (currentLvl == 6)
            {
                if (Vector2.Distance(player.transform.position, lastSun.position) <= 30)
                {
                    dialogueManager.StartDialogue(dialogues[4]);
                    currentLvl += 1;
                    lvlTimer = Time.time;
                    musicSource.loop = false;
                    //musicSource.clip = musics[5];
                    musicSource.volume = 0.45f;
                }
            }
            if (currentLvl == 7 && !dialogueManager.animator.GetBool("IsOpen") && Time.time - lvlTimer > 5f)
            {
                dialogueManager.StartDialogue(dialogues[5]);
                currentLvl += 1;
                lvlTimer = Time.time;
            }
        }
    }
}
