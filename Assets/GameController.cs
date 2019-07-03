using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public GameObject[] carModels;
    public GameObject[] bots;

    public GameObject player;
    public GameObject playerPrefab;

    public GameObject shield;
    public GameObject shield2;

    public int numberOfBots;
    public int chosenCar = 0;

    int offSet = 50;
    int tempOffset = 10;

    [SerializeField]
    public SortedDictionary<string, int> scores;

    void Awake()
    {
        SpawnPlayer();
        SpawnBots();
    }

    void Update()
    {

    }

    void SpawnPlayer()
    {
        //player spawn
        player = spawnObj(playerPrefab, getRandPos(1), Quaternion.identity);
        player.name = "player";
        SetScore(player.name, 0);

        GameObject playerCar = spawnObj(carModels[chosenCar], player.transform.position, Quaternion.identity);
        playerCar.transform.parent = player.transform;

        playerCar = spawnObj(shield, new Vector3(player.transform.position.x,
                                                player.transform.position.y + 0.3f,
                                                player.transform.position.z + 1.3f), Quaternion.identity);
        playerCar.transform.parent = player.transform;


        playerCar = spawnObj(shield2, new Vector3(player.transform.position.x,
                                               player.transform.position.y + 0.3f,
                                               player.transform.position.z + 1.3f), Quaternion.identity);
        playerCar.transform.parent = player.transform;

    }

    void SpawnBots()
    {
        //bots spawn
        for (int i = 0; i < numberOfBots; i++)
        {
            bots[i] = spawnObj(bots[i], getRandPos(i + 1), Quaternion.identity);
            GameObject botCar = spawnObj(carModels[(int)UnityEngine.Random.Range(0, 8)], bots[i].transform.position, Quaternion.identity);
            botCar.transform.parent = bots[i].transform;

            //arabanın önüne shield spawn eder
            botCar = spawnObj(shield, new Vector3(bots[i].transform.position.x,
                                                  bots[i].transform.position.y + 0.3f,
                                                  bots[i].transform.position.z + 1.3f), Quaternion.identity);
            botCar.transform.parent = bots[i].transform;

            botCar = spawnObj(shield2, new Vector3(bots[i].transform.position.x,
                                                  bots[i].transform.position.y + 0.3f,
                                                  bots[i].transform.position.z + 1.3f), Quaternion.identity);
            botCar.transform.parent = bots[i].transform;
            
            bots[i].transform.forward = getRandPos(i + 1);
            bots[i].name = "Bot" + (i + 1).ToString();
            SetScore(bots[i].name, 0);
        }
    }

    Vector3 getRandPos(int i)
    {
        //UnityEngine.Random.seed = DateTime.UtcNow.Millisecond;
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
        Vector3 randomPlace = new Vector3(UnityEngine.Random.Range(-i * 5, i * 5),
                                          1f,
                                          UnityEngine.Random.Range(-i * 5, i * 5));

        return randomPlace;
    }

    GameObject spawnObj(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        return (GameObject)GameObject.Instantiate(obj, pos, quaternion);
    }

    void InitScores()
    {
        if (scores != null)
            return;

        scores = new SortedDictionary<string, int>();
    }
    
    public void SetScore(string name, int value)
    {
        InitScores();
        if (scores.ContainsKey(name) == false)
        {
            scores.Add(name, value);
        }
        
        scores[name] = value;
    }

    public int GetScore(string name)
    {
        InitScores();

        if (scores.ContainsKey(name) == false)
            return 0;

        return scores[name];
    }

    public void UpdateScore(string name, int amount)  
    {
        InitScores();

        int currentScore = scores[name];
        SetScore(name, currentScore + amount);
    }

    private void OnGUI()
    {
        var scoreBoard = scores.OrderByDescending(x => x.Value);

        GUI.skin.label.fontSize = 40;
        foreach( KeyValuePair<string, int> kvp in scoreBoard)
        {
            GUI.Label(new Rect(Screen.width - 250, Screen.height - Screen.height + tempOffset, Screen.width, Screen.height), kvp.Key + " " + kvp.Value);
            tempOffset += offSet;
        }
        tempOffset = 10;
    }
}
