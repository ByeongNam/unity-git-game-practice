using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ProcessRotation();
    }
    void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space)){
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }

    }
    void ProcessRotation(){
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(-rotationThrust);
        }
        else if(Input.GetKey(KeyCode.D)){
            ApplyRotation(rotationThrust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rigidBody.AddTorque(new Vector3(0, 0, rotationThisFrame * Time.deltaTime));
    }
}
