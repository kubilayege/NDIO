using UnityEngine;

public class botCar : MonoBehaviour
{
    GameController gc;
    GameObject target;
    Rigidbody rb;
    GameObject scaler;
    GameObject scaler2;
    Vector3 direction; //yön bilgisi

    public int score;
    public float knockbackcoff = 1000.0f;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();

        scaler = transform.GetChild(1).gameObject;
        scaler2 = transform.GetChild(2).gameObject;
    }

    void Update()
    {
        Move();
    }


    void Move()
    {
        int id = GetBotID(this.name);
        target = FindClosest(id);
        try { direction = (target.transform.position - transform.position).normalized * 7 * Time.deltaTime; } catch { }

        transform.forward += new Vector3(Mathf.Lerp(transform.position.x, direction.x, 1.2f),
                                        0,
                                        Mathf.Lerp(transform.position.z, direction.z, 1.2f));

        rb.MovePosition(transform.position + direction);

    }
    GameObject FindClosest(int thisID)
    {
        var maxdistance = 0.0f;
        var distance = 0.0f;

        GameObject closest = gc.player;

        for (int i = 0; i < gc.bots.Length; i++)
        {
            if (i == thisID) { continue; }
            try { distance = (gc.bots[i].transform.position - gc.bots[thisID].transform.position).magnitude; } catch { }
            if (distance > maxdistance) { maxdistance = distance; closest = gc.bots[i].gameObject; }
        }

        try { distance = (gc.player.transform.position - gc.bots[thisID].transform.position).magnitude; } catch { }
        if (distance > maxdistance) { return gc.player; }

        return closest;
    }
    int GetBotID(string name)
    {
        for (int i = 0; i < gc.bots.Length; i++)
        {
            try { if (gc.bots[i].name == name) return i; } catch { }
        }
        return -1;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shield")
        {
            KnockBack();
        }

        if (other.gameObject.tag == "Car")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
            KillSomeone();
        }


    }

    void KnockBack()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * knockbackcoff);
    }
    void KillSomeone()
    {
        score += 1;
        scaler.gameObject.transform.localScale += new Vector3(0.5f, 0, 0);
        scaler2.gameObject.transform.localScale += new Vector3(0.5f, 0, 0);
    }
}
