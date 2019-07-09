using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //scene manager / environment

public class Rocket : MonoBehaviour
{
    //rocket member variables
    [SerializeField] float rotationForce = 100f;
    [SerializeField] float thrustForce = 1800f;
    //AudioFX
    [SerializeField] AudioClip primaryEngineSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip dyingSound;
    //ParticleFX
    [SerializeField] ParticleSystem engineParticleFX;
    [SerializeField] ParticleSystem successParticleFX;
    [SerializeField] ParticleSystem dyingParticleFX;

    //game controller member variable
    //I believe this could work as an int, need to call LoadScene starting at 0 index *MAYBE*
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
    }


    //game controller (based off tags in game)
    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.alive) { return; }//prevents additional return statements/evaluations

        switch (collision.gameObject.tag)//switch statement based off tag
        {
            case "Friendly"://do nothing
                print("friendly contact");
                break;

            case "Finish":
                SuccessConditionMet();
                break;

            default:
                DeathConditionMet();
                break;
        }
    }

    private void DeathConditionMet()
    {
        print("unfriendly contact");
        whatLevelWeAreOn = "FirstLevel";//member variable storing level
        state = State.dying;
        audioSource.Stop();//stops all sounds before
        audioSource.PlayOneShot(dyingSound);//playing the death sound
        Invoke("LevelController", 2f);
    }

    private void SuccessConditionMet()
    {
        print("Hit Finish");
        whatLevelWeAreOn = "SecondLevel";//changing member variable
        //state change
        state = State.transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        //We are using Invoke to delay the function execution, like a timeout
        //This method requires functions to be called as string types however
        Invoke("LevelController", 1f);
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
            engineParticleFX.Play();
            ApplyThrustNorth();
        }
        else if (Input.GetKey(KeyCode.S)) //can trust while rotating
        {
            engineParticleFX.Play();
            ApplyThrustSouth();
        }
        else
        {
            audioSource.Stop();
            //engineParticleFX.Stop();
        }
    }

    private void ApplyThrustSouth()
    {
        //adding force relative to the object's direction
        //Vector3 struct used to add force upwards
        rigidBody.AddRelativeForce(Vector3.down * thrustForce);
        //engineParticleFX.Play();

        if (!audioSource.isPlaying) //method to allow no overlapping play
        {
            audioSource.PlayOneShot(primaryEngineSound);
        }
    }

    private void ApplyThrustNorth()
    {
        //adding force relative to the object's direction
        //Vector3 struct used to add force upwards
        rigidBody.AddRelativeForce(Vector3.up * thrustForce);
        //engineParticleFX.Play();

        if (!audioSource.isPlaying) //method to allow no overlapping play
        {
            audioSource.PlayOneShot(primaryEngineSound);
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
