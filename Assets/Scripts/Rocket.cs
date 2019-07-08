using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //scene manager / environment

public class Rocket : MonoBehaviour
{
    //member variables
    [SerializeField] float rotationForce = 100f;
    [SerializeField] float thrustForce = 1800f;
    [SerializeField] string whatLevelWeAreOn = "FirstLevel";

    //accessing the rigidbody component on rocket
    //that we added using the unity GUI
    Rigidbody rigidBody;
    AudioSource audioSource;

    //game state
    enum State { alive, dying, transcending };

    //using a variable to store one of the values of our above enumerator
    //this is still considered a member variable and is initializing the state
    // as being alive.
    State state = State.alive;


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
        if (state == State.alive)
        {
            Thrust();
            Rotation();
        }
        else
        {
            audioSource.Stop();
        }
    }


    //game controller (based off tags in game)
    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.alive) { return; }//prevents additional return statements/evaluations

        //print("Collided");
        switch (collision.gameObject.tag)//switch statement based off tag
        {
            case "Friendly"://do nothing
                print("friendly contact");
                break;

            case "Finish":
                print("Hit Finish");
                whatLevelWeAreOn = "SecondLevel";//creating member variable to store level
                //state change
                state = State.transcending;
                //LevelController(1);
                //We are using Invoke to delay the function execution, like a timeout
                //This method requires functions to be called as string types however
                Invoke("LevelController", 1f);
                break;

            default:
                print("unfriendly contact");
                whatLevelWeAreOn = "FirstLevel";//member variable storing level
                state = State.dying;

                Invoke("LevelController", 1f);
                
                //LevelController(0);
                //TODO: kill player
                break;
        }
    }

    private void LevelController()
    {
        //string levelToChangeTo = whatLevelWeAreOn.ToString();
        SceneManager.LoadScene(whatLevelWeAreOn);
    }

    //player controls
    private void Thrust()
    {
        //GetKey() - auto / GetKeyDown()/Up() -for single fire usage
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) //can trust while rotating
        {
            //adding force relative to the object's direction
            //Vector3 struct used to add force upwards
            rigidBody.AddRelativeForce(Vector3.up * thrustForce);

            if (!audioSource.isPlaying) //method to allow no overlapping play
            {
                audioSource.Play();
            }
        }
        else if (Input.GetKey(KeyCode.S)) //can trust while rotating
        {
            //adding force relative to the object's direction
            //Vector3 struct used to add force upwards
            rigidBody.AddRelativeForce(Vector3.down * thrustForce);

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

}
