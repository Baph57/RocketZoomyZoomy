using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //member variables
    [SerializeField] float rotationForce = 100f;
    [SerializeField] float thrustForce = 2f;

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
            rigidBody.AddRelativeForce(Vector3.up * thrustForce);

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

        
        float rotationSpeedThisFrame = rotationForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            //permanent unity object/component
            //Vector3 allows for adding force forward, still a struct

            transform.Rotate(Vector3.forward * rotationSpeedThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward * rotationSpeedThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Collided");
        switch (collision.gameObject.tag)//switch statement based off tag
        {
            case "Friendly":
                print("friendly contact");
                break;

            default:
                print("unfriendly contact");
                //TODO: kill player
                break;
        }
    }
}
