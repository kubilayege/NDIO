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
    GameObject scaler;
    GameObject scaler2;
    float knockbackcoff = 1000.0f;
    int score;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        scaler = GameObject.Find("Player(Clone)/Shield(Clone)");
        scaler2 = GameObject.Find("Player(Clone)/Shield2(Clone)");
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
