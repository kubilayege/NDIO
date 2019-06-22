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
    Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        
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
        //rb.velocity = transform.forward * carSpeed * Time.deltaTime;
    }

    void Move()
    {
        RaycastHit hit;
        Ray camDir = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(camDir, out hit);
        direction = (hit.point - transform.position).normalized * carSpeed * Time.deltaTime;
        //direction = (new Vector3((Input.mousePosition.x - cam.WorldToScreenPoint(transform.position).x),
        //                                0,
        //                                (Input.mousePosition.y - cam.WorldToScreenPoint(transform.position).y))) * carSpeed * Time.deltaTime;

       // direction = direction - transform.forward * carSpeed * Time.deltaTime;
        //rb.AddForce(direction, ForceMode.Force);
        rb.MovePosition(direction);
        transform.rotation.SetLookRotation(direction.normalized, Vector3.up);
    }
}
