using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlickering : MonoBehaviour
{

    Transform mainLight;
    Transform flickerLight;
    Light2D mainLightComponent;
    Light2D flickerLightComponent;

    [SerializeField] [Range(0, 5)] float minIntensity;
    [SerializeField] [Range(0, 5)] float maxIntensity;


    // Start is called before the first frame update
    void Start()
    {
        mainLight = this.transform.GetChild(0);
        flickerLight = this.transform.GetChild(1);
        mainLightComponent = mainLight.GetComponent<Light2D>();
        flickerLightComponent = flickerLight.GetComponent<Light2D>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) //this is while(true)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, 0.2f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
