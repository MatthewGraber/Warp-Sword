using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static MusicManager Instance { get; private set; }

    [SerializeField] AudioSource startingMusic;
    [SerializeField] AudioSource bossMusic;

    [SerializeField] AudioClip deathMusic;


    private bool fadingStarting = false;
    bool bossActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        bossMusic.volume = 0;
        bossMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingStarting)
        {
            startingMusic.volume -= Time.deltaTime;
            if (startingMusic.volume <= 0)
            {
                fadingStarting = false;
                startingMusic.Stop();
                startingMusic.volume = 1;
            }
        }
    }


    public void FadeStarting()
    {
        if (startingMusic != null)
        {
            fadingStarting = true;
        }
    }


    public void BossTime()
    {
        if (!bossActive) {
            bossMusic.time = 0;
            bossMusic.volume = 1;
            bossActive = true;
        }
        // startingMusic.Stop();
        
    }

    public void DeathMusic()
    {
        bossMusic.volume = 0;
        startingMusic.clip = deathMusic;
        startingMusic.volume = 1;
        startingMusic.Play();
    }
}
