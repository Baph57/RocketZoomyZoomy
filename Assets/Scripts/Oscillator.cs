using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[DisallowMultipleComponent] this makes it so only one object can have this script
//in a scene, I have commented it out because I may want multiple oscillators
public class Oscillator : MonoBehaviour
{
    //assigned default value to prevent error, but seems pointless
    [SerializeField] Vector3 oscillationVector = new Vector3( -60f, 0f, 0f);
    [SerializeField] float oscillationPeriod = 4f;


    //TODO remove later
    //you can have these fields in-line or stacked, whitespace ignored
    //0 is origin position, 1 is final destination
    [Range(0,1)][SerializeField]float movementVelocity;

    Vector3 StartingPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(oscillationPeriod >= Mathf.Epsilon)
        {
        //by dividing time by 2, we ensure we have one cycle every 2 seconds
        float amountOfCycles = Time.time / oscillationPeriod;

        const float Tau = Mathf.PI * 2f; // about 6.28
        float rawSineWave = Mathf.Sin(amountOfCycles * Tau);
        movementVelocity = (rawSineWave + 1) / 2;

        Vector3 StartingPositionOffset = oscillationVector * movementVelocity;
        transform.position = StartingPosition + StartingPositionOffset;
        }
    }
}
