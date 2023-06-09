using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 충돌체를 생성해 무기를 줏을수 있도록 한다.
/// 루팅했으면 충돌체는 제거.
/// 무기를 다시 버릴수도 있어야 하며, 버렸을경우, 충돌체를 다시 넣는다
/// Gun에 줏은 무기를 넣어주게 된다.
/// </summary>
public class InteractiveWeapon : MonoBehaviour
{
    public GunData gunData; // 총의 데이터
    public Transform muzzleTransform; // 탄알이 발사될 위치
    public Vector3 handPosition;
    public Vector3 handRotation;
    public GameObject muzzleFlashEffect;
    public GameObject leftHandPivot;
    
    public int totalMagCapacity;
    public int fullMagCapacity;
    public int currentMagCapacity;

    private AimBehaviour aimBehaviour;
    private LineRenderer lineRenderer;
    private GameObject player, gameController;
    private ShootBehaviour playerInventory;
    private BoxCollider weaponCollider;
    private SphereCollider interactiveCollider;
    private Rigidbody weaponRigidbody;
    private bool pickable;
    
    
    //UI
    public GameObject screenHUD;
    public WeaponUIManager weaponHUD;
    private Transform pickupHUD;
    public Text pickupHUD_Text;


    private void Awake()
    {
        handPosition = gunData.handPosition;
        handRotation = gunData.handRotation;
        gameObject.name = gunData.weaponName;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        foreach (Transform tr in transform)
        {
            tr.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        aimBehaviour = GetComponent<AimBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<ShootBehaviour>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        gameController = GameObject.FindGameObjectWithTag("GameController");

        totalMagCapacity = gunData.totalMagCapacity;
        fullMagCapacity = gunData.fullMagCapacity;
        currentMagCapacity = fullMagCapacity;
        
        if (weaponHUD == null)
        {
            if (screenHUD == null)
            {
                screenHUD = GameObject.Find("ScreenHUD");
            }

            weaponHUD = screenHUD.GetComponent<WeaponUIManager>();
        }

        if (pickupHUD == null)
        {
            pickupHUD = gameController.transform.Find("PickupHUD");
        }
        
        // 인터렉션을 위한 충돌체 설정
        // child에 collider를 넣는이유는 본체에는 trigger를 달기위해서이다
        weaponCollider = transform.GetChild(0).AddComponent<BoxCollider>();
        weaponRigidbody = gameObject.AddComponent<Rigidbody>();
        CreateInteraciveRadius(weaponCollider.center);
        
        //pickupHUD.gameObject.SetActive(false);

        if (muzzleTransform == null)
        {
            muzzleTransform = transform.Find("FireTransform");
        }
    }

    private void CreateInteraciveRadius(Vector3 center)
    {
        interactiveCollider = gameObject.AddComponent<SphereCollider>();
        interactiveCollider.center = center; 
        interactiveCollider.radius = 1;
        interactiveCollider.isTrigger = true;
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
            pickupHUD_Text.text = gunData.weaponName;
        }
    }

    
    private void UpdateHUD()
    {
        weaponHUD.UpdateWeaponHUD(gunData.weaponSprite, currentMagCapacity,fullMagCapacity,totalMagCapacity);
    }

    public void Toggle(bool active)
    {
        if (active)
        {
            SoundManager.Instance.PlayAudioSourceAtPoint(gunData.pickSound, transform.position,0.5f);
        }
        weaponHUD.WeaponUIToggle(active);
        UpdateHUD();
    }

    public void Drop()
    {
        gameObject.SetActive(true);
        transform.position += Vector3.up;
        weaponRigidbody.isKinematic = false;
        this.transform.parent = null;
        CreateInteraciveRadius(weaponCollider.center);
        this.weaponCollider.enabled = true;
        weaponHUD.WeaponUIToggle(false);
    }

    public bool StartReload()
    {
        if (currentMagCapacity == fullMagCapacity || totalMagCapacity == 0)
        {
            return false;
        }
        else if (totalMagCapacity < fullMagCapacity - currentMagCapacity)
        {
            currentMagCapacity += totalMagCapacity;
            totalMagCapacity = 0;
        }
        else
        {
            totalMagCapacity = totalMagCapacity - fullMagCapacity + currentMagCapacity;
            currentMagCapacity = fullMagCapacity;
        }
        SoundManager.Instance.PlayAudioSourceAtPoint(gunData.reloadSound,muzzleTransform.position,0.5f);
        return true;
    }

    public void EndReload()
    {
        UpdateHUD();
    }

    public bool Shoot()
    {
        if (currentMagCapacity > 0)
        {
            currentMagCapacity--;
            SoundManager.Instance.PlayAudioSourceAtPoint(gunData.shotSound, muzzleTransform.position,0.5f);
            UpdateHUD();
            return true;
        }
        else if (currentMagCapacity == 0)
        {
            SoundManager.Instance.PlayAudioSourceAtPoint(gunData.noBulletSound, muzzleTransform.position,0.5f);
        }

        return false;
    }

    public void ResetBullet()
    {
        currentMagCapacity = gunData.fullMagCapacity;
        totalMagCapacity = gunData.totalMagCapacity;
    }


    private void Update()
    {
        if(this.pickable == true && Input.GetButtonDown(PlayerInput.pickButtonName))
        {
            weaponRigidbody.isKinematic = true;
            weaponCollider.enabled = false;
            playerInventory.AddWeapon(this);
            Destroy(interactiveCollider);
            this.Toggle(true);
            this.pickable = false;
            
            TogglePickUpHUD(false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject != player &&
            Vector3.Distance(transform.position, player.transform.position) <= 5)
        {
            SoundManager.Instance.PlayAudioSourceAtPoint(gunData.dropSound, transform.position,0.1f);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player && playerInventory.isActiveAndEnabled)
        {
            pickable = true;
            TogglePickUpHUD(true);
        }
    }

   
}
