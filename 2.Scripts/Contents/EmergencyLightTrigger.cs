using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLightTrigger : MonoBehaviour
{
    public EmergencyLight emergencyLight;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (emergencyLight.isEmergency == true)
            {
                return;
            }

            emergencyLight.isEmergency = true;
            emergencyLight.Initialize();
            emergencyLight.StartEmergency();
            SoundManager.Instance.PlayAudioSource(SoundManager.Instance.BGM_fadeA_audio, 11,0.5f);
        }
    }

    
}
