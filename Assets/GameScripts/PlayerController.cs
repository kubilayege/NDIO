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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        
    }
    
    void Update()
    {
        MoveForward();
        if (Input.GetMouseButton(0))
        {
            Move();
        }
        if (Input.GetMouseButtonDown(1))
        {
            KnockBack();
        }
    }

    void KnockBack()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * 1000.0f);
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

        transform.rotation.SetLookRotation(direction.normalized, Vector3.up);
    }
}
