using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //accessing the rigidbody component on rocket
    //that we added using the unity GUI
    Rigidbody rigidBody;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //using GetComponent to assign rigidBody to the actual component
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotation();
    }

    private void Thrust()
    {
        //GetKey() - auto / GetKeyDown()/Up() -for single fire usage
        if (Input.GetKey(KeyCode.Space)) //can trust while rotating
        {
            //adding force relative to the object's direction
            //Vector3 struct used to add force upwards
            rigidBody.AddRelativeForce(Vector3.up);

            if (!audioSource.isPlaying) //method to allow no overlapping play
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotation()
    {

        rigidBody.freezeRotation = true; //taking control of rotation manually

        if (Input.GetKey(KeyCode.A))
        {
            //permanent unity object/component
            //Vector3 allows for adding force forward, still a struct
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("D - Right");
            transform.Rotate(-Vector3.forward);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

}
