using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Actions/Attack")]
public class AttackAction : Action
{
    private float startShootTimer = 1.0f;
    public override void UpdateAction(StateController controller)
    {
        startShootTimer += Time.deltaTime;
        if (startShootTimer >= controller.baseStats.shotDelay)
        {
            ShotCast(controller);
            controller.enemyAnimation.enemyAnimator.SetTrigger(AnimatorKey.Shooting);
            startShootTimer = 0;
        }
    }

   
    public override void OnEnabledAction(StateController controller)
    {
        startShootTimer = 0f;
        controller.focusSight = true;
        controller.enemyNav.destination = controller.transform.position;
        controller.isAiming = true;
        controller.enemyAnimation.enemyAnimator.SetBool(AnimatorKey.Crouch, false);
        controller.needToMovePosition = controller.aimTarget.transform.position;
    }
    public override void ExitAction(StateController controller)
    {
        controller.isAiming = false;
    }

    private void ShotEffect(StateController controller)
    {
        GameObject muzzleFlash = EffectManager.Instance.EffectOneShot(1, Vector3.zero);
        muzzleFlash.transform.SetParent(controller.gunMuzzle);
        muzzleFlash.transform.localPosition = Vector3.zero;
        Destroy(muzzleFlash, 1.0f);
        
        SoundManager.Instance.PlayAudioSourceAtPoint(10, controller.gunMuzzle.position,0.5f);
    }

    private void ShotCast(StateController controller)
    {
        Vector3 shotErrorRate = Vector3.zero;
        shotErrorRate +=  Random.Range(-controller.baseStats.shotErrorRate, controller.baseStats.shotErrorRate)
                          * controller.transform.right;
        shotErrorRate +=  Random.Range(-controller.baseStats.shotErrorRate, controller.baseStats.shotErrorRate)
                          * controller.transform.up;

        Vector3 shotDirection = controller.aimTarget.transform.position - controller.gunMuzzle.position;
        shotDirection = shotDirection.normalized + shotErrorRate;

        Ray ray = new Ray(controller.transform.position + Vector3.up * 1.8f, controller.aimTarget.transform.position - controller.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, controller.baseStats.lookRadius, controller.baseStats.targetMask))
        {
            ShotEffect(controller);
        }
    }


}
