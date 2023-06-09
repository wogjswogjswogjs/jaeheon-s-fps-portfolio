using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 발자국 소리를 출력.
/// </summary>
public class PlayerFootStep : MonoBehaviour
{
   public int stepSound;
   private Animator myAnimator;
   private int index;
   private Transform leftFoot, rightFoot;
   private float dist;
   private int groundedParamHash, aimParamHash, speedParamHash;
   private bool grounded;

   private float lastTime;
   private float lastTimeUpdate;
   public enum FOOT
   {
      LEFT,
      RIGHT
   }

   private FOOT step = FOOT.LEFT;

   private void Start()
   {
      lastTimeUpdate = 0.04f;
      myAnimator = GetComponent<Animator>();
      
      leftFoot = myAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
      rightFoot = myAnimator.GetBoneTransform(HumanBodyBones.RightFoot);

      speedParamHash = Animator.StringToHash(AnimatorKey.Speed);
      groundedParamHash = Animator.StringToHash(AnimatorKey.Grounded);
      aimParamHash = Animator.StringToHash(AnimatorKey.Aim);
   }

   private void PlayFootStep()
   {
      SoundManager.Instance.PlayAudioSourceAtPoint(stepSound, transform.position, 0.1f);
   }

   private void Update()
   {
      grounded = myAnimator.GetBool(groundedParamHash);
      float factor = 0.1f;
      
      if (grounded && myAnimator.GetFloat(speedParamHash) > 0.1f &&
          Time.time >= lastTime + lastTimeUpdate)
      {
         lastTime = Time.time;
         switch (step)
         {
            case FOOT.LEFT :
               dist = leftFoot.position.y - transform.position.y;
               if (dist <= factor)
               {
                  PlayFootStep();
                  step = FOOT.RIGHT;
               }
               break;
            case FOOT.RIGHT :
               dist = rightFoot.position.y - transform.position.y;
               if (dist <= factor)
               {
                  PlayFootStep();
                  step = FOOT.LEFT;
               }
               break;
         }
      }
   }
}
