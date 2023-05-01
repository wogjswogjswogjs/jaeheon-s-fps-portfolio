using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthHUD : MonoBehaviour
{
    private Camera mainCamera;
    private Image hud, bar;
    private Color originalColor, noAlphaColor;

    private void Start()
    {
        hud = transform.Find("HUD").GetComponent<Image>();
        bar = transform.Find("Bar").GetComponent<Image>();
        mainCamera = Camera.main;
        originalColor = noAlphaColor = hud.color;
        noAlphaColor.a = 0f;
        
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }
        
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
        
        
    }
}
