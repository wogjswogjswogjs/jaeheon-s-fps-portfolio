using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Money : MonoBehaviour
{
    private SphereCollider moneyCollider;
    private GameObject player, gameController;
    private bool pickable; // 주울수 있는 상태인가
    public int money;
    
    // UI
    private Transform pickupHUD;
    public Text pickupHUD_Text;
    
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        player = GameObject.FindGameObjectWithTag("Player");

        if (pickupHUD == null)
        {
            pickupHUD = gameController.transform.Find("PickupHUD");
        }

        
    }
    
    private void TogglePickUpHUD(bool toggle)
    {
        pickupHUD.gameObject.SetActive(toggle);
        if (toggle)
        {
            pickupHUD.position = transform.position + Vector3.up * 0.5f;
            Vector3 direction = player.GetComponent<PlayerController>().playerComponents.thirdPersonCameraScript
                .transform.forward;
            
            direction.y = 0;
            pickupHUD.rotation = Quaternion.LookRotation(direction);
            pickupHUD_Text.text =money.ToString() + "$";
        }
    }

    public delegate void OnPickMoney(int money);

    public event OnPickMoney OnPickMoneyEventHandler;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            pickable = true;
            TogglePickUpHUD(true);
        }
        
        if (pickable == true && Input.GetButtonDown(PlayerInput.pickButtonName))
        {
            StageManager.Instance.AddMoney(money);
            pickable = false;
            TogglePickUpHUD(false);
            OnPickMoneyEventHandler?.Invoke(money);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            pickable = false;
            TogglePickUpHUD(false);
        }
    }

    
}
