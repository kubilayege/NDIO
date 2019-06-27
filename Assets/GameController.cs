using UnityEngine;
using System;
public class GameController : MonoBehaviour
{
    public GameObject[] carModels;
    public GameObject[] bots;

    public GameObject player;
    public GameObject playerPrefab;

    public GameObject shield;
    public GameObject shield2;

    public int[] scoreBoard;
    public int numberOfBots;
    public int chosenCar = 0;

    int tempScore;
    int tempKillScore;
    int offSet = 50;
    int tempOffset = 10;

    void Start()
    {
        SpawnPlayer();
        SpawnBots();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateHighScore();
    }

    void SpawnPlayer()
    {
        //player spawn
        player = spawnObj(playerPrefab, getRandPos(1), Quaternion.identity);

        GameObject playerCar = spawnObj(carModels[chosenCar], player.transform.position, Quaternion.identity);
        playerCar.transform.parent = player.transform;

        playerCar = spawnObj(shield, new Vector3(player.transform.position.x,
                                                player.transform.position.y + 0.3f,
                                                player.transform.position.z + 1.25f), Quaternion.identity);
        playerCar.transform.parent = player.transform;


        playerCar = spawnObj(shield2, new Vector3(player.transform.position.x,
                                               player.transform.position.y + 0.3f,
                                               player.transform.position.z + 1.25f), Quaternion.identity);
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

            //arabanın önüne banner spawn eder
            botCar = spawnObj(shield, new Vector3(bots[i].transform.position.x,
                                                  bots[i].transform.position.y + 0.3f,
                                                  bots[i].transform.position.z + 1.25f), Quaternion.identity);
            botCar.transform.parent = bots[i].transform;

            botCar = spawnObj(shield2, new Vector3(bots[i].transform.position.x,
                                                  bots[i].transform.position.y + 0.3f,
                                                  bots[i].transform.position.z + 1.25f), Quaternion.identity);
            botCar.transform.parent = bots[i].transform;

            bots[i].transform.forward = getRandPos(i + 1);
            bots[i].name = "Bot" + (i + 1).ToString();
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

    //TODO kimin score'u arttıysa ona aktarılacak
    void GetKill(int score)
    {
        tempKillScore = score;
    }

    //TODO isimler eklenecek.
    void CalculateHighScore()
    {
        for (int i = 0; i < numberOfBots + 1; i++)
        {
            if (scoreBoard[i] < scoreBoard[i + 1])
            {
                tempScore = scoreBoard[i + 1];
                scoreBoard[i + 1] = scoreBoard[i];
                scoreBoard[i] = tempScore;
            }
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 40;
        for (int i = 0; i < numberOfBots + 1; i++)
        {
            GUI.Label(new Rect(Screen.width - 70, Screen.height - Screen.height + tempOffset, Screen.width, Screen.height), scoreBoard[i].ToString());
            tempOffset += offSet;
        }
        tempOffset = 10;
    }
}
