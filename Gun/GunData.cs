using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GunData")]
public class GunData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public Vector3 handPosition;
    public Vector3 handRotation;
    
    public WEAPONTYPE WEAPONTYPE = WEAPONTYPE.NONE;
    public WEAPONMODE weaponMode = WEAPONMODE.ONESHOT;
    
    [Header("Sound")]
    public int shotSound;
    public int reloadSound;
    public int pickSound;
    public int dropSound;
    public int noBulletSound;

   
    
    
    
    [Header("Bullet")]
    public int totalMagCapacity; // 전체 탄창 용령
    public int fullMagCapacity; // 탄창 용량
    public float recoilAngle;
    public float damage; 
    [Header("FireTime")]
    public float timeBetFire; // 탄알 발사 간격
}
