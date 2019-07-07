﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject[] bots;
    public GameObject[] carModels;
    public Material[] carMaterials;
    Material[] objmats;

    public GameObject playerPreview;
    public GameObject playerPrefab;
    public GameObject player;

    public GameObject shield;
    public GameObject shield2;
    GameObject UI;
    
    string nicknameInput;

    public int numberOfBots;
    int modelIdx;
    int colorIdx;

    int offSet = 50;
    int tempOffset = 10;

    public float carSpeeds=10.0f;

    [SerializeField]
    public SortedDictionary<string, int> scores;

    void Awake()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
        playerPreview = spawnObj(carModels[0], new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity);
        StartCoroutine(rotatePreview());
    }

    IEnumerator rotatePreview()
    {
        while(UI.activeInHierarchy != false)
        {
            playerPreview.transform.Rotate(0, 20 * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }

    }

    public void ChangePreviewCarColor()
    {
        colorIdx = (int)UI.transform.GetChild(3).gameObject.GetComponent<Slider>().value;
        Material[] objmats = playerPreview.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
        objmats[1] = carMaterials[colorIdx];

        playerPreview.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = objmats;

    }

    public void ChangePreviewCarModel()
    {
        modelIdx = (int)UI.transform.GetChild(2).gameObject.GetComponent<Slider>().value;
        Destroy(playerPreview, 0.01f);
        playerPreview = spawnObj(carModels[modelIdx], new Vector3(0.0f, 0.1f, 0.0f), playerPreview.transform.rotation);
    }

    void Update()
    {

    }


    public void StartGame()
    {
        nicknameInput = UI.transform.GetChild(1).gameObject.GetComponent<InputField>().text;
        
        if (nicknameInput.Length > 2 )
        {
            UI.SetActive(false);
            Destroy(playerPreview);
            InitScores();
            SpawnPlayer();
            SpawnBots();
        }
        
    }

    void SpawnPlayer()
    {
        //player spawn
        player = spawnObj(playerPrefab, getRandPos(1), Quaternion.identity);
        player.name = nicknameInput;
        SetScore(player.name, 0);

        GameObject playerCar = spawnObj(playerPreview, player.transform.position, Quaternion.identity);
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
        Vector3 randomPlace = new Vector3(UnityEngine.Random.Range(-i * 10, i * 10),
                                          1f,
                                          UnityEngine.Random.Range(-i * 10, i * 10));

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
        if(scores != null)
        {
            var scoreBoard = scores.OrderByDescending(x => x.Value);

            GUI.skin.label.fontSize = 40;
            foreach (KeyValuePair<string, int> kvp in scoreBoard)
            {
                GUI.Label(new Rect(Screen.width - 250, Screen.height - Screen.height + tempOffset, Screen.width, Screen.height), kvp.Key + " " + kvp.Value);
                tempOffset += offSet;
            }
            tempOffset = 10;
        }

    }    
}
