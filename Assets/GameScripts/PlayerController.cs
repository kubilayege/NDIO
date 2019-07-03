using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float carSpeed;
    [SerializeField]
    Vector3 direction;
    Rigidbody rb;
    [SerializeField]
    Camera cam;
    GameController gc;
    [SerializeField]
    GameObject scaler;
    [SerializeField]
    GameObject scaler2;
    float knockbackcoff = 1000.0f;
    int score;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        scaler = transform.GetChild(1).gameObject;
        scaler2 = transform.GetChild(2).gameObject;
    }
    
    void Update()
    {
        MoveForward();
        if (Input.GetMouseButton(0) && transform.position.y < 0.2)
        {
            Move();
        }
    }

    void MoveForward()
    {
        rb.MovePosition(transform.position+(transform.forward * carSpeed * Time.deltaTime));
        cam.transform.position =  new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z - 7.0f);
      
    }

    void Move()
    {
        RaycastHit hit;
        Ray camDir = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(camDir, out hit);
        
        direction = (hit.point - transform.position).normalized * carSpeed * Time.deltaTime;
        
        //transform.forward += direction;
        transform.forward += new Vector3(Mathf.Lerp(transform.position.x, direction.x, 1.2f),
                                        0,
                                        Mathf.Lerp(transform.position.z, direction.z, 1.2f));

        rb.MovePosition(transform.position + direction);

        //transform.rotation.SetLookRotation(direction.normalized, Vector3.up);
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
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    void KnockBack()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * knockbackcoff);
    }

    void KillSomeone(GameObject other)
    {
        gc.UpdateScore(this.name, gc.GetScore(other.name) + 10);
        gc.scores[other.name] = 0;
        scaler.gameObject.transform.localScale += new Vector3(0.5f + other.transform.GetChild(1).localScale.x, 0, 0);
        scaler2.gameObject.transform.localScale += new Vector3(0.5f + other.transform.GetChild(2).localScale.x, 0, 0);

    }
}
