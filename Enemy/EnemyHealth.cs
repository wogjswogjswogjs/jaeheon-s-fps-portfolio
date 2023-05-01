using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : HealthBase
{
    public float health = 100.0f;
    public Transform healthHUD;
    public int deathSound;
    public int hitSound;
    public GameObject hurtPrefab;

    private RectTransform healthBar, placeHolderBar;
    private Text healthText;
    private float totalHealth;
    private float originalBarScale;
    private bool isCritical;

    private void Awake()
    {
       /* totalHealth = health;
        
        healthBar = healthHUD.Find("HealthBar/Bar").GetComponent<RectTransform>();
        healthBar = hud.transform.Find("Bar").GetComponent<RectTransform>();
        originalBarScale = healthBar.sizeDelta.x;
        controller = GetComponent<StateController>();
        gameController = GameObject.FindWithTag("GameController");*/
    }

    private void UpdateHealthBar()
    {
        float scaleFactor = health / totalHealth;
        healthBar.sizeDelta = new Vector2(scaleFactor * originalBarScale, healthBar.sizeDelta.y);
    }

    private void Death()
    {
        IsDead = true;
        Destroy(gameObject);
    }
    public override void TakeDamage(Vector3 damagePos, Vector3 damageDir, float damage)
    {
        health -= damage;
        UpdateHealthBar();

        if (health <= 0)
        {
            Death();
        }
    }
}
