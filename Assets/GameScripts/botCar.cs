using UnityEngine;

public class botCar : MonoBehaviour
{
    GameController gc;
    [SerializeField]
    GameObject target;
    Rigidbody rb;
    GameObject scaler;
    GameObject scaler2;
    Vector3 direction; //yön bilgisi

    public int score;
    float knockbackcoff = 250.0f;

    void Start()
    {
        InitializeVariables();
    }

    void InitializeVariables()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        if (transform.GetChild(1) != null)
        {
            Debug.Log(this.name);
            scaler = transform.GetChild(1).gameObject;
            scaler2 = transform.GetChild(2).gameObject;
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        rb = GetComponent<Rigidbody>();

        int id = gc.GetBotID(this.name);
        target = FindClosest(id);
        try { direction = (target.transform.position - transform.position).normalized * gc.carSpeeds * Time.deltaTime; } catch { }

        transform.forward += new Vector3(Mathf.Lerp(transform.position.x, direction.x, 1.2f),
                                        0,
                                        Mathf.Lerp(transform.position.z, direction.z, 1.2f));

        rb.MovePosition(transform.position + direction);

    }

    GameObject FindClosest(int thisID)
    {
        var mindistance = 200.0f;
        var distance = 0.0f;

        GameObject closest = gc.player;

        for (int i = 0; i < gc.bots.Length; i++)
        {
            if (i == thisID) { continue; }
            if (gc.bots[i] != null) { distance = (gc.bots[i].transform.position - gc.bots[thisID].transform.position).magnitude; } 
            if (distance < mindistance) { mindistance = distance; closest = gc.bots[i].gameObject; }
        }

        if (gc.player != null && gc.bots[thisID] != null) { distance = (gc.player.transform.position - gc.bots[thisID].transform.position).magnitude; } 
        if (distance < mindistance) { return gc.player; }

        return closest;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shield")
        {
            KnockBack();
        }

        if (other.gameObject.tag == "Car")
        {
            KillSomeone(other.gameObject.transform.parent.gameObject);
        }

    }

    void KnockBack()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * knockbackcoff);
    }

    void KillSomeone(GameObject other)
    {
        gc.UpdateScore(this.name, gc.GetScore(other.name) + 10);

        if (other.name == gc.nicknameInput) { gc.PlayerDead(gc.GetScore(other.name)); Debug.Log("I killed Player"); }
        else { gc.ReSpawnBot(gc.GetBotID(other.name)); }

        scaler.gameObject.transform.localScale += new Vector3(0.5f+ other.transform.GetChild(1).localScale.x/8 , 0, 0);
        scaler2.gameObject.transform.localScale += new Vector3(0.5f+ other.transform.GetChild(2).localScale.x/8 , 0, 0);
    }
}
