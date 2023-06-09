using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 무기를 획득하면 획득한 무기를 UI를 통해 보여주고
/// 현재 잔탄량과 전체 소지할수 있는 총알량을 출력
/// </summary>
public class WeaponUIManager : MonoBehaviour
{
    public Color bulletColor = Color.white;
    public Color emptyBulletColor = Color.black;
    private Color noBulletColor;

    [SerializeField] private Image weaponImg;
    [SerializeField] private Text bulletText;
    
    private void Start()
    {
        noBulletColor = new Color(0f, 0f, 0f, 0f);
        
        if (weaponImg == null)
        {
            weaponImg = transform.Find("WeaponHUD/WeaponImg").GetComponent<Image>();
        }

        if (bulletText == null)
        {
            bulletText = transform.Find("WeaponHUD/AmmoText").GetComponent<Text>();
        }

        WeaponUIToggle(false);
    }
    
    public void WeaponUIToggle(bool active)
    {
        weaponImg.transform.parent.gameObject.SetActive(active); // == WeaponHUD
    }
    
    public void UpdateWeaponHUD(Sprite weaponSprite, int currentMagCapacity, int fullMagCapacity, int totalMagCapacity)
    {
        if (weaponSprite != null && weaponImg.sprite != weaponSprite)
        {
            weaponImg.sprite = weaponSprite;
            weaponImg.type = Image.Type.Filled;
            weaponImg.fillMethod = Image.FillMethod.Horizontal;
        }
        bulletText.text = currentMagCapacity + "/" +  totalMagCapacity;
    }
}
