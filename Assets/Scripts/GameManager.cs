using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject messenger;
    public TMP_Text messageBox;

    [SerializeField] Light sun;
    bool dimingSun;

    public enum GameState
    {
        Playing,
        Ended
    }

    public GameState state = GameState.Playing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        messageBox = messenger.GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dimingSun)
        {
            sun.intensity -= Time.deltaTime/3;
            if (sun.intensity < 0.2)
            {
                sun.intensity = 0.2f;
                dimingSun = false;
            }
        }

        if (state == GameState.Ended && Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }


    // Called when the player dies
    public void PlayerDied()
    {
        state = GameState.Ended;
        messageBox.text = "You've met with a terrible fate, haven't you?\n\nPress 'R' to restart";
        MusicManager.Instance.DeathMusic();
    }


    // Called when we're in the endgame
    public void GameEnd()
    {
        Shrek.Instance.Activate();
        MusicManager.Instance.BossTime();
        dimingSun = true;
    }


    // Called when the player wins
    public void Victory()
    {
        state = GameState.Ended;
        messageBox.text = "You've defeated Shrek!\n\nPress 'R' to restart";
        MusicManager.Instance.VictoryMusic();
    }


    public void Message(string message, float delay)
    {
        StartCoroutine(ShowMessage(message, delay));
    }


    IEnumerator ShowMessage(string message, float delay)
    {
        messageBox.text = message;
        yield return new WaitForSeconds(delay);
        messageBox.text = "";
    }
}
