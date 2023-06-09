using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthHUD : MonoBehaviour
{
    public float healthDamp = 1.0f;
    private float healthTimer;
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
        healthTimer += Time.deltaTime;
        if (healthTimer >= healthDamp)
        {
            float from = healthTimer - healthTimer;
            float to = healthTimer;
            hud.color = Color.Lerp(originalColor, noAlphaColor, from / to);
            bar.color = Color.Lerp(originalColor, noAlphaColor, from / to);
        }

        if (healthTimer >= healthDamp)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetVisible()
    {
        gameObject.SetActive(true);
        healthTimer = 0f;
        hud.color = bar.color = originalColor;
    }
}
