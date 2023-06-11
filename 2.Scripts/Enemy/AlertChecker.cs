using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlertChecker : MonoBehaviour
{
    [Range(0,50)]public float alertRadius;
    public LayerMask alertMask = LayerMask.GetMask("Enemy");
    private Vector3 current;
    private bool alert;
    public int extraWaves = 1;

    private void Start()
    {
        InvokeRepeating("PingAlert",1,1);
    }

    private void AlertNearBy(Vector3 position, Vector3 target, int wave = 0)
    {
        if (wave > this.extraWaves) return;
        
        Collider[] targetsInViewRadius = Physics.OverlapSphere(position, alertRadius, alertMask);

        foreach (Collider obj in targetsInViewRadius)
        {
            obj.SendMessageUpwards("AlertCallBack", target, SendMessageOptions.DontRequireReceiver);
            AlertNearBy(obj.transform.position, target, wave + 1);
        }
    }

    public void RootAlertNearBy(Vector3 position)
    {
        current = position;
        alert = true;
    }

    void PindAlert()
    {
        if (alert)
        {
            alert = false;
            AlertNearBy(current, current);
        }
    }
}
