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
    //Level member variables
    [SerializeField] float delayBetweenStageSwitch = 2f;
    //debugging member variable
    [SerializeField] bool debugCollisionToggle;

    //game controller member variable
    //I believe this could work as an int, need to call LoadScene starting at 0 index *MAYBE*
    [SerializeField] int currentSceneIndex;

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

        //current level - GetActiveScene not allowed inside MonoBehaviour
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //hotkeys designed to streamline testing game
        if (Debug.isDebugBuild)
        {
        DebugKeys();
        }
        if (state == State.alive)
        {
            Thrust();
            Rotation();
        }
    }

    void DebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            //currentSceneIndex++;
            //TODO: have this actually load the right level
            SuccessConditionMet();
            LevelController();
        }
        if (Input.GetKey(KeyCode.C))
        {
            debugCollisionToggle = !debugCollisionToggle;
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
                if (!debugCollisionToggle)//debug key check, should collisions register?
                {
                DeathConditionMet();
                }
                break;
        }
    }

    private void DeathConditionMet()
    {
        print("unfriendly contact");
        currentSceneIndex = 0;//member variable storing level
        state = State.dying;
        audioSource.Stop();//stops all sounds before
        audioSource.PlayOneShot(dyingSound);//playing the death sound
        dyingParticleFX.Play();
        Invoke("LevelController", delayBetweenStageSwitch);
    }

    private void SuccessConditionMet()
    {
        print("Hit Finish");
        currentSceneIndex++;//changing member variable
        //state change
        state = State.transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticleFX.Play();
        //We are using Invoke to delay the function execution, like a timeout
        //This method requires functions to be called as string types however
        Invoke("LevelController", delayBetweenStageSwitch);
    }

    private void LevelController()
    {
        //this stores the current level's index in an integer
        //every game has a build order for scenes! Manipulate like arrays!
        
        //var baseLevel = SceneManager.GetSceneByBuildIndex(0); woops, WET code
        if(currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            //print(desiredNextLevel + " desired next level ");
            print(SceneManager.sceneCountInBuildSettings + "scene count");
            //desiredNextLevel = 0;
            print(currentSceneIndex + " CURSCEINDX");
            currentSceneIndex = 0;
        }
        //string levelToChangeTo = whatLevelWeAreOn.ToString();
        //TODO: Refactoring changed the behavior of 'currentSceneIndex'
        //It's no longer the currentScene, but rather the variable controller
        //for level switching
        SceneManager.LoadScene(currentSceneIndex);
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
            engineParticleFX.Stop();
        }
    }

    private void ApplyThrustSouth()
    {
        //adding force relative to the object's direction
        //Vector3 struct used to add force upwards
        rigidBody.AddRelativeForce(Vector3.down * thrustForce * Time.deltaTime);
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
        rigidBody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
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
