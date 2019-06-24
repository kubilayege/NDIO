using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject[] bots;
    public GameObject player;

    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject[] carModels;

    [SerializeField]
    int numberOfBots;
    [SerializeField]
    int chosenCar=0;

    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {

        //player spawn
        player = spawnObj(playerPrefab, getRandPos(1), Quaternion.identity);
        GameObject playerCar = spawnObj(carModels[chosenCar], player.transform.position , Quaternion.identity);

        playerCar.transform.parent = player.transform;
        

        //bots spawn
        for (int i = 0; i < numberOfBots; i++)
        {
            bots[i] = spawnObj(bots[i], getRandPos(i+1), Quaternion.identity);
            GameObject botCar = spawnObj(carModels[(int)UnityEngine.Random.Range(0,8)], bots[i].transform.position, Quaternion.identity);
            

            botCar.transform.parent = bots[i].transform;
            bots[i].transform.forward = getRandPos(i+1);
            bots[i].name = "Bot" + (i+1).ToString();
        }
    }

    Vector3 getRandPos(int i)
    {
        UnityEngine.Random.seed = DateTime.UtcNow.Millisecond;
        Vector3 randomPlace = new Vector3(UnityEngine.Random.Range(-10.0f - i*4, 10.0f + i*4),
                                          1f,
                                          UnityEngine.Random.Range(-10.0f - i*4, 10.0f + i*4)); 

        return randomPlace;
    }

    GameObject spawnObj(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        return (GameObject)GameObject.Instantiate(obj, pos, quaternion);
    }
}
