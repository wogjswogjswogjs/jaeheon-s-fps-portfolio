using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 숨을 만한 곳을 찾아주는 컴포넌트
/// 플레이어보다 멀리있는 건 제외
/// </summary>
public class CoverLookUp : MonoBehaviour
{
    private List<Vector3[]> coverSpots;
    private GameObject[] covers;
    private List<int> coverHashCodes;
    private Dictionary<float, Vector3> filteredSpors;

    private Collider[] GetBoundObject(int layerMask, float radius)
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Bound"));
        return col;
    }
    
    

    public void Start()
    {
        Collider[] go = GetBoundObject(11,10.0f);
        foreach (var VARIABLE in go)
        {
            Debug.Log(VARIABLE.gameObject.name);
        }
    }

    private void ProcessPoint(List<Vector3> vector3s, Vector3 nativePoint, float range)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(nativePoint, out hit, range, NavMesh.AllAreas))
        {
            vector3s.Add(hit.position);
        }
    }

    

}
