using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject[] bots;
    [SerializeField]
    GameObject[] carModels;

    [SerializeField]
    int numberOfBots;
    [SerializeField]
    int chosenCar;

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
        GameObject player = spawnObj(playerPrefab, getRandPos(), Quaternion.identity);
        GameObject playerCar = spawnObj(carModels[chosenCar], player.transform.position , Quaternion.identity);

        playerCar.transform.parent = player.transform;
        

        //bots spawn
        for (int i = 0; i < numberOfBots; i++)
        {
            bots[i] = spawnObj(bots[i], getRandPos(), Quaternion.identity);
            GameObject botCar = spawnObj(carModels[(int)UnityEngine.Random.Range(0,8)], bots[i].transform.position, Quaternion.identity);
            

            botCar.transform.parent = bots[i].transform;
            bots[i].transform.forward = getRandPos();
        }
    }

    Vector3 getRandPos()
    {
        UnityEngine.Random.seed = DateTime.UtcNow.Millisecond;
        Vector3 randomPlace = new Vector3(UnityEngine.Random.Range(-38, 38),
                                          1f,
                                          UnityEngine.Random.Range(-38, 38)); 

        return randomPlace;
    }

    GameObject spawnObj(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        return (GameObject)GameObject.Instantiate(obj, pos, quaternion);
    }
}
