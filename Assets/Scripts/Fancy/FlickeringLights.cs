using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlickeringLights : MonoBehaviour
{


    //Able to customize
    //1. How long the light stays off for
    //2. How long the entire interval for the lights is
    //Ex. You set the interval to "2", and the off time to "1", the light will be off for 1 second every 2 second interval
    [SerializeField] float flickeringInterval;
    [SerializeField] float offTime;

    float lightTimer;

    SpotLight lightToFlicker;
    // Start is called before the first frame update
    void Awake()
    {
        lightToFlicker = GetComponent<SpotLight>();
    }

    // Update is called once per frame
    void Update()
    {
        lightTimer -= Time.deltaTime;
        if(lightTimer <= 0)
        {
            //Restart the interval
        }
    }
}
