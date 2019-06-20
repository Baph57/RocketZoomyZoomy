using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //accessing the rigidbody component on rocket
    //that we added using the unity GUI
    Rigidbody rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        //using GetComponent to assign rigidBody to the actual component
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessUserInput();
    }

    private void ProcessUserInput()
    {
        //GetKey() - auto / GetKeyDown()/Up() -for single fire usage
        if (Input.GetKey(KeyCode.Space)) //can trust while rotating
        {
            //adding force relative to the object's direction
            //Vector3 struct used to add force upwards
            rigidBody.AddRelativeForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            print("A - Left");
            
            //permanent unity object/component
            //Vector3 allows for adding force forward, still a struct
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("D - Right");
            transform.Rotate(Vector3.forward);
        }
    }
}
