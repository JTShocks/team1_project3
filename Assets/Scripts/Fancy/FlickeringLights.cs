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
    [SerializeField] float whenToFlickerTime;
    [SerializeField] float offTime;
    [SerializeField] int numberOfFlickers;

    Coroutine flickerRoutine;

    float lightTimer;

    Light lightToFlicker;
    // Start is called before the first frame update
    void Awake()
    {
        lightToFlicker = GetComponent<Light>();
        
    }

    // Update is called once per frame
    void Update()
    {

        lightTimer -= Time.deltaTime;

        if(lightTimer <= 0)
        {
            if(flickerRoutine != null)
            {
                StopCoroutine(flickerRoutine);
            }
            //Restart the interval
            lightTimer = flickeringInterval;
            flickerRoutine = StartCoroutine(FlickerRoutine());
        }
    }

    IEnumerator FlickerRoutine()
    {
        for(int i = 0; i < numberOfFlickers; i++)
        {
            yield return new WaitForSeconds(whenToFlickerTime);
            lightToFlicker.enabled = false;
            yield return new WaitForSeconds(offTime);
            lightToFlicker.enabled = true;
        }

        
    }
}
