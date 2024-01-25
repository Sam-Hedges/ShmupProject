using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public LevelGeneration levelGenerator;
    public MovePlayer player;
    public bool inTile;
    public GameObject[] coinSpawnPoints;
    public GameObject coinGroup, coin,coinGroupBig, nitro, rock, rock2,spike;
    public GameObject[] coins;
    public GameObject[] cars;

    public GameObject[] obstaclesSpawn;
    public GameObject[] obstacles;
    public Transform obstaclesParent,coinsParent, nitroParent;
    public int[] obstaclesTest;
    public bool emptyRoad,carTile;
    public GameObject[] nitroSpawns;
    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = GameObject.FindObjectOfType<LevelGeneration>();
        coins = new GameObject[10];
        obstacles = new GameObject[obstaclesTest.Length];
        if (emptyRoad == false)
        {
            SpawnObstacles();
        }

        CoinSpawn();
        NitroSpawn();
    }
    //Generates obstacles depending on the type of tile it is
    public void SpawnObstacles()
    {
        if (carTile == false)
        {
            for (int i = 0; i < obstaclesTest.Length; i++)
            {
                int spawnRND = Random.Range(0, 100);
                int obstaclechance = levelGenerator.difficulty * Random.Range(1, 11);
                if (spawnRND < 20 + obstaclechance && obstaclesTest[i] != 1)
                {
                    obstaclesTest[i] = 1;
                }
                else
                {
                    obstaclesTest[i] = 0;
                }
                if (obstaclesTest[1] == 1 && obstaclesTest[0] == 1 && obstaclesTest[2] == 1)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                    {
                        obstaclesTest[0] = 0;
                    }
                    else if (rnd == 0)
                    {
                        obstaclesTest[1] = 0;
                    }
                    else
                    {
                        obstaclesTest[2] = 0;
                    }
                }
                if (obstaclesTest[3] == 1 && obstaclesTest[4] == 1 && obstaclesTest[5] == 1)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                    {
                        obstaclesTest[5] = 0;
                    }
                    else if (rnd == 1)
                    {
                        obstaclesTest[4] = 0;
                    }
                    else
                    {
                        obstaclesTest[3] = 0;
                    }

                }
                if (obstaclesTest[6] == 1 && obstaclesTest[7] == 1 && obstaclesTest[8] == 1)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                    {
                        obstaclesTest[6] = 0;
                    }
                    else if (rnd == 1)
                    {
                        obstaclesTest[7] = 0;
                    }
                    else
                    {
                        obstaclesTest[8] = 0;
                    }
                }
            }

            for (int j = 0; j < obstaclesTest.Length; j++)
            {
                if (obstaclesTest[j] == 1)
                {
                    int obstacleRND = Random.Range(0, 2);
                    if (obstacleRND == 0)
                    {
                        int rockRND = Random.Range(0, 2);
                        if (rockRND == 1)
                        {
                            obstacles[j] = Instantiate(rock, new Vector3(obstaclesSpawn[j].transform.position.x, obstaclesSpawn[j].transform.position.y + 0.5f, obstaclesSpawn[j].transform.position.z), Quaternion.identity, obstaclesParent);
                        }
                        else
                        {
                            obstacles[j] = Instantiate(rock2, new Vector3(obstaclesSpawn[j].transform.position.x, obstaclesSpawn[j].transform.position.y + 0.5f, obstaclesSpawn[j].transform.position.z), Quaternion.identity, obstaclesParent);
                        }
                    }
                    else
                    {
                        obstacles[j] = Instantiate(spike, obstaclesSpawn[j].transform.position, Quaternion.identity, obstaclesParent);
                    }

                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                int spawnRND = Random.Range(0, 3);
                if (spawnRND == 0 && obstaclesTest[i] != 1)
                {
                    obstaclesTest[i] = 1;
                }
                else
                {
                    obstaclesTest[i] = 0;
                }
                if (obstaclesTest[0] == 1 && obstaclesTest[1] == 1 && obstaclesTest[2] == 1)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                    {
                        obstaclesTest[0] = 0;
                    }
                    else if (rnd == 0)
                    {
                        obstaclesTest[1] = 0;
                    }
                    else
                    {
                        obstaclesTest[2] = 0;
                    }
                }
            }
            for (int j = 0; j < obstaclesTest.Length; j++)
            {
                if (obstaclesTest[j] == 1)
                {
                    int carRND = Random.Range(0, 3);
                    int spawnRND = Random.Range(0, 6);
                    if (carRND == 0)
                    {
                        obstacles[j] = Instantiate(cars[carRND], new Vector3(obstaclesSpawn[j].transform.position.x, obstaclesSpawn[j].transform.position.y + 0.5f, obstaclesSpawn[j].transform.position.z + spawnRND), Quaternion.identity, obstaclesParent);
                    }
                    else if (carRND == 1)
                    {
                        obstacles[j] = Instantiate(cars[carRND], new Vector3(obstaclesSpawn[j].transform.position.x, obstaclesSpawn[j].transform.position.y + 0.5f, obstaclesSpawn[j].transform.position.z + spawnRND), Quaternion.identity, obstaclesParent);
                    }
                    if (carRND == 2)
                    {
                        obstacles[j] = Instantiate(cars[carRND], new Vector3(obstaclesSpawn[j].transform.position.x, obstaclesSpawn[j].transform.position.y + 0.5f, obstaclesSpawn[j].transform.position.z + spawnRND), Quaternion.identity, obstaclesParent);
                    }
                }
            }
        }
    }
    
    public void CoinSpawn()
    {
        for (int i = 0; i < obstaclesTest.Length; i++)
        {
            if (obstaclesTest[i] == 0)
            {
                int rnd = Random.Range(0, 100);
                int coinchance = levelGenerator.difficulty * Random.Range(1, 6);
                if (rnd < 10 + coinchance)
                {
                    int coinRnd = Random.Range(0, 3);
                    if (coinRnd == 0)
                    {
                        coins[i] = Instantiate(coinGroup, new Vector3(obstaclesSpawn[i].transform.position.x, obstaclesSpawn[i].transform.position.y + 1f, obstaclesSpawn[i].transform.position.z), Quaternion.identity, coinsParent);

                    }
                    if (coinRnd == 1)
                    {
                        coins[i] = Instantiate(coinGroupBig, new Vector3(obstaclesSpawn[i].transform.position.x, obstaclesSpawn[i].transform.position.y + 1f, obstaclesSpawn[i].transform.position.z), Quaternion.identity, coinsParent);

                    }
                    else
                    {
                        coins[i] = Instantiate(coin, new Vector3(obstaclesSpawn[i].transform.position.x, obstaclesSpawn[i].transform.position.y + 1f, obstaclesSpawn[i].transform.position.z), Quaternion.identity, coinsParent);
                    }
                    obstaclesTest[i] = 2;
                }
                
            }
        }
    }

    public void NitroSpawn()
    {
        for (int i = 0; i < obstaclesTest.Length; i++)
        {
            if (obstaclesTest[i] == 0)
            {
                int rnd = Random.Range(1, 100);
                if (rnd < 5)
                {
                    Instantiate(nitro, new Vector3(obstaclesSpawn[i].transform.position.x, obstaclesSpawn[i].transform.position.y + 1f, obstaclesSpawn[i].transform.position.z), Quaternion.identity, nitroParent);
                    obstaclesTest[i] = 3;
                }
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Leaving section");
            levelGenerator.SpawnTile();
            for(int i = 0; i < coins.Length; i++)
            {
                Destroy(coins[i]);
            }
            for(int i = 0; i< obstacles.Length; i++)
            {
                Destroy(obstacles[i]);
            }
            Destroy(gameObject,1);
            inTile = false;
        }
        
    }
}
