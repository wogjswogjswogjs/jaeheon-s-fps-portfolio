using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
   public class DamageInfo
   {
      public Vector3 damagePos, damageDir;
      public float damage;
      public Collider bodyPart;
      public GameObject hitEffect;

      public DamageInfo(Vector3 damagePos, Vector3 damageDir, float damage, GameObject hitEffect = null)
      {
         this.damagePos = damagePos;
         this.damageDir = damageDir;
         this.damage = damage;
         this.hitEffect = hitEffect;
      }
   }

   [HideInInspector] public bool IsDead;
   protected Animator myAnimator;

   public virtual void TakeDamage(Vector3 damagePos, Vector3 damageDir, float damage, GameObject hitEffect = null)
   {
      
   }

   public void HitCallBack(DamageInfo damageInfo)
   {
      this.TakeDamage(damageInfo.damagePos,damageInfo.damageDir,damageInfo.damage, damageInfo.hitEffect);
   }
}
