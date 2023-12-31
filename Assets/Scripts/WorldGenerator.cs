using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] GameObject StartingIsland;
    [SerializeField] GameObject BossIsland;

    [SerializeField] List<GameObject> RandomIslands;

    [SerializeField] GameObject Water;

    [SerializeField] GameObject player;
    [SerializeField] Vector3 playerStartPos;

    // [SerializeField] GameObject Boss;
    // [SerializeField] Vector3 bossStartPos;


    // How large the world, including water, will be
    [SerializeField] int WorldSize = 40;

    // How large of an area islands will spawn in
    // A value of 5 will create a 5x5 grid of islands
    [SerializeField] int islandGridSize = 15;
    [SerializeField] int emptyChance = 4;

    // Space between islands
    int islandSpacing = 300;

    int waterSize = 150;

    // Start is called before the first frame update
    void Start()
    {
        BuildWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildWorld()
    {
        // Create water
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                Instantiate(Water, new Vector3(i*waterSize, 0, j*waterSize), transform.rotation);
            }
        }

        // Create islands
        int gridStart = (WorldSize*waterSize - islandGridSize*islandSpacing)/2;
        List<Vector3> islandCoords = new List<Vector3>();
        for (int i = 0; i < islandGridSize; i++)
        {
            for (int j = 0; j < islandGridSize; j++)
            {
                islandCoords.Add(new Vector3(gridStart + i * islandSpacing, 0, gridStart + j * islandSpacing));
            }
        }

        // Set the boss location
        BossIsland.transform.SetPositionAndRotation(islandCoords[islandCoords.Count / 2], BossIsland.transform.rotation);
        islandCoords.RemoveAt(islandCoords.Count / 2);

        // Spawn the start location
        int rand = Random.Range(0, islandCoords.Count);
        Instantiate(StartingIsland, islandCoords[rand], transform.rotation);

        // Set the player postion
        player.transform.SetPositionAndRotation(islandCoords[rand] + playerStartPos, player.transform.rotation);
        islandCoords.RemoveAt(rand);

        foreach (Vector3 loc in islandCoords)
        {
            rand = Random.Range(0, RandomIslands.Count*emptyChance);
            if (rand % emptyChance == 0)
            {
                Instantiate(RandomIslands[rand/emptyChance], loc, transform.rotation);
            }
        }

    }
}
