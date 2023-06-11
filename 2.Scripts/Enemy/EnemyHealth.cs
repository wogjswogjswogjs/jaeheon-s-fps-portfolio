using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : HealthBase
{
    public int deathSound;
    public int hitSound;
    public int hitEffect;
    
    public bool headShot;
    
    public float health = 100.0f;
    public GameObject hurtPrefab;
    public GameObject enemyHealthHUD;
    private EnemyHealthHUD enemyHealthHUDScript;

    private Transform hud;
    private RectTransform healthBar, placeHolderBar;
    private Text healthText;
    private float totalHealth;
    private float originalBarScale;
    private bool isCritical;

    private Animator anim;
    private EnemyController controller;
    private GameObject gameController;
    private void Awake()
    {
        hud = Instantiate(enemyHealthHUD, transform).transform;
        
        if (!hud.gameObject.activeSelf)
        {
            hud.gameObject.SetActive(true);
        }

        totalHealth = health;
        healthBar = hud.transform.Find("Bar").GetComponent<RectTransform>();
        enemyHealthHUDScript = hud.GetComponent<EnemyHealthHUD>();
        originalBarScale = healthBar.sizeDelta.x;
        
        anim = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void UpdateHealthBar()
    {
        float scaleFactor = health / totalHealth;
        healthBar.sizeDelta = new Vector2(scaleFactor * originalBarScale, healthBar.sizeDelta.y);
    }
    
    
    private void Death()
    {
        IsDead = true;
        anim.SetBool(AnimatorKey.Aim, false);
        anim.SetBool(AnimatorKey.Crouch, false);
        anim.enabled = false;
        Destroy(hud.gameObject);
    }
    public override void TakeDamage(Vector3 damagePos, Vector3 damageDir, float damage, GameObject hitEffect = null)
    {
        EffectManager.Instance.EffectOneShot(this.hitEffect, damagePos);
        health -= damage;
        
        if (!IsDead)
        {
            anim.SetTrigger("Hit");
            enemyHealthHUDScript.SetVisible();
            UpdateHealthBar();
            controller.isFeelAlert = true;
            controller.needToMovePosition = controller.aimTarget.transform.position;
        }
        
        if (health <= 0)
        {
            controller.enabled = false;
            anim.applyRootMotion = true;
            anim.SetBool("Dead", true);
            Destroy(gameObject, 1.5f);
            //Death();
        }
    }
}
