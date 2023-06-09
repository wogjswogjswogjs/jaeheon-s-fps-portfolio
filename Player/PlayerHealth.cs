using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 플레이어의 생명력을 담당
/// 피격시 피격이펙트를 표시하거나, UI업데이트를 한다.
/// 죽었을 경우, 모든 동작스크립트 동작을 멈춘다.
/// </summary>
public class PlayerHealth : HealthBase
{
    public int deathSound;
    public int hitSound;
    
    public float health = 100.0f;
    public Transform healthHUD;
    public GameObject hurtPrefab;

    private RectTransform healthBar, placeHolderBar;
    public Text healthText;
    private float totalHealth;
    private float originalBarScale;
    private bool isCritical;
    private RectTransform hitPivot;
    public Image hitImg;
    private float hitImgAlphaColor; 
        
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        totalHealth = health;
        healthHUD = GameObject.Find("HealthHUD").transform;
        
        hitPivot = healthHUD.Find("HitPivot").GetComponent<RectTransform>();
        hitImg = hitPivot.Find("HitImage").GetComponent<Image>();
        healthBar = healthHUD.Find("HealthBar/Bar").GetComponent<RectTransform>();
        placeHolderBar = healthHUD.Find("HealthBar/Placeholder").GetComponent<RectTransform>();
        healthText = healthHUD.Find("HealthBar/Text").GetComponent<Text>();
        originalBarScale = healthBar.sizeDelta.x;
        healthText.text = "" + health;
    }

    private void Update()
    {
        if (placeHolderBar.sizeDelta.x > healthBar.sizeDelta.x)
        {
            placeHolderBar.sizeDelta =
                Vector2.Lerp(placeHolderBar.sizeDelta, healthBar.sizeDelta, 2.0f * Time.deltaTime);
        }
    }

    public bool IsFullLife()
    {
        return Mathf.Abs(health - totalHealth) < float.Epsilon;
    }

    private void UpdateHealthBar()
    {
        healthText.text = "" + health;
        float scaleFactor = health / totalHealth;
        healthBar.sizeDelta = new Vector2(scaleFactor * originalBarScale, healthBar.sizeDelta.y);
    }

    private void Death()
    {
        IsDead = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.tag = "Untagged";   
        healthHUD.gameObject.SetActive(false);
        healthHUD.parent.Find("WeaponHUD").gameObject.SetActive(false);
        myAnimator.SetBool(AnimatorKey.Aim,false);
        myAnimator.SetFloat(AnimatorKey.Speed,0);
        foreach (var behaviour in GetComponentsInChildren<BaseBehaviour>())
        {
            behaviour.enabled = false;
        }

        SoundManager.Instance.PlayAudioSourceAtPoint(deathSound, transform.position, 5.0f);
    }

    public override void TakeDamage(Vector3 damagePos, Vector3 damageDir, float damage, GameObject hitEffect = null)
    {
        health -= damage;
        UpdateHealthBar();

        if (health <= 0)
        {
            Death();
        }

        SetHitImageRotation(hitPivot,Camera.main.transform.forward, damagePos - transform.position);
        SoundManager.Instance.PlayAudioSourceAtPoint(hitSound, damagePos, 0.1f);
    }
    
    private void SetHitImageRotation(RectTransform hitImagePivot, Vector3 camForward, Vector3 shotDirection)
    {
        camForward.y = 0;
        shotDirection.y = 0;
        float rotation = Vector3.SignedAngle(shotDirection, camForward, Vector3.up);
        Vector3 newRotation = hitImagePivot.rotation.eulerAngles;
        newRotation.z = rotation;
        hitImagePivot.rotation = Quaternion.Euler(newRotation);
        StartCoroutine(SetHitImageAlphaColor());
    }

    private IEnumerator SetHitImageAlphaColor()
    {
        var hitImgColor = hitImg.color;
        hitImgColor.a = 1.0f;
        hitImg.color = hitImgColor;
        while (hitImgColor.a > 0)
        {
            hitImgColor.a -= Time.deltaTime;
            hitImg.color = hitImgColor;
            yield return null;
        }
    }

}
