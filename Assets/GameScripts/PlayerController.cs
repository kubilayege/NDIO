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

        transform.forward = direction;

        rb.MovePosition(transform.position + direction);

        transform.rotation.SetLookRotation(direction.normalized, Vector3.up);
    }
}
