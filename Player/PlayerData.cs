using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private GameObject playerPrefab;

    public GameObject GetPlayerPrefab()
    {
        return playerPrefab;
    }
}