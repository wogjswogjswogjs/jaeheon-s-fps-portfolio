using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLight : MonoBehaviour
{
    public bool isEmergency;
    private Light emergencyLight;
    public float maxIntensity;
    
    public void Initialize()
    {
        emergencyLight = GetComponent<Light>();
        maxIntensity = 5.0f;
    }

    public void StartEmergency()
    {
        isEmergency = true;
        StartCoroutine(LightFadeInOut());
    }

    public void EndEmergency()
    {
        isEmergency = false;
        emergencyLight.intensity = 0.0f;
    }

    public IEnumerator LightFadeInOut()
    {
        while (isEmergency)
        {
            emergencyLight.intensity = Mathf.PingPong(Time.time, maxIntensity);
            yield return null;
        }
    }
}
