using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject messenger;
    public TMP_Text messageBox;

    [SerializeField] Light sun;
    bool dimingSun;

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
    }


    // Called when the player dies
    public void PlayerDied()
    {
        Message("You've met with a terrible fate, haven't you?", 8.0f);
        MusicManager.Instance.DeathMusic();
    }


    // Called when we're in the endgame
    public void GameEnd()
    {
        Shrek.Instance.Activate();
        MusicManager.Instance.BossTime();
        dimingSun = true;
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
