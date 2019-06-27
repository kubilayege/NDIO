using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botCar : MonoBehaviour
{
    GameController gc;
    GameObject target;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    
    void Move()
    {
        int id = GetBotID(this.name);
        target = FindClosest(id);

        rb.MovePosition(transform.position + ( ( target.transform.position - transform.position).normalized * 10.0f * Time.deltaTime) );

        transform.forward += new Vector3(Mathf.Lerp(transform.position.x, target.transform.position.x, 1.2f),
                                        0,
                                        Mathf.Lerp(transform.position.z, target.transform.position.z, 1.2f));
    }
    GameObject FindClosest(int thisID)
    {
        var maxdistance = 0.0f;
        var distance = 0.0f;

        GameObject closest= gc.player;

        for (int i = 0; i < gc.bots.Length; i++)
        {
            if(i == thisID) { continue; }
            distance = (gc.bots[i].transform.position - gc.bots[thisID].transform.position).magnitude; 
            if(distance> maxdistance) { maxdistance = distance; closest = gc.bots[i].gameObject; }
        }

        distance = (gc.player.transform.position - gc.bots[thisID].transform.position).magnitude;
        if(distance > maxdistance) { return gc.player; }

        return closest;
    }
    int GetBotID(string name)
    {
        for(int i = 0; i < gc.bots.Length; i++)
        {
            if(gc.bots[i].name == name) return i;
        }
        return -1;
    }
}
