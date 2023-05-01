using System.Collections.Generic;
using _2.Scripts.Player;
using _2.Scripts.Tag;
using Unity.VisualScripting;
using UnityEngine;

namespace _2.Scripts.Stage
{
    public class StageManager : MonoBehaviour
    {
        
        #region Camera

        internal ThirdPersonCamera camera;
        
        #endregion


        #region Player

        [SerializeField] private Transform playerSpawnLocation;
        
        private const string playerDataPath = "Data/Player/" + nameof(PlayerData);
        private PlayerData playerData;
        private PlayerController playerController;
        
        #endregion
        
        #region Eenmy

        [SerializeField] private List<Transform> spawnLocations = new List<Transform>();
        private List<StateController> enemies = new List<StateController>();
        
        #endregion

   
        private void Awake()
        {
            GameSetting();
        }
        
        /// <summary>
        /// [재헌] 게임내에 필요한 카메라, 플레이어 스폰, 적 스폰의 역할을 합니다.
        /// </summary>
        private void GameSetting()
        {
            PlayerSetting();    
            CameraSetting();
        }


        #region Camera
        
        /// <summary>
        /// [재헌] 카메라 세팅에 필요한 초기화 함수를 실행 합니다.
        /// </summary>
        private void CameraSetting()
        {
            if (Camera.main != null)
            {
                camera = Camera.main.GetComponent<ThirdPersonCamera>();
                camera.Initialize(playerController.transform);
            }
        }

        #endregion
        
        #region Player Manager
        
        /// <summary>
        /// [재헌] PlayerData에서 프리팹을 가져와 플레이어를 스폰하고
        /// 플레이어 매니저가 플레이어를 컨트롤하는 스크립트를 캐싱함.
        /// </summary>
        private void PlayerSetting()
        {
           LoadPlayerResource();
           GetPlayerSpawnLocation();
           PlayerSpawn();
        }

        /// <summary>
        /// [재헌] Resources 폴더에 저장된 PlayerData 스크립터블 오브젝트를 Load해서 캐싱
        /// </summary>
        private void LoadPlayerResource()
        {
            playerData = Resources.Load(playerDataPath, typeof(ScriptableObject)) as PlayerData;
        }

        /// <summary>
        /// [재헌] 플레이어가 스폰될 위치를 클라이언트에서 불러와 캐싱하고 그 포지션에 불러온 PlayerData에 저장된 프리팹을 생성
        /// </summary>
        private void GetPlayerSpawnLocation()
        {
            playerSpawnLocation = GameObject.FindObjectOfType<PlayerSpawnLocation>().transform;
        }
        
        private void PlayerSpawn()
        {
            var playerObj = Instantiate(playerData.GetPlayerPrefab());
            
            playerObj.transform.localPosition = playerSpawnLocation.localPosition;
            playerObj.transform.localRotation = playerSpawnLocation.localRotation;
            
            playerController = playerObj.GetComponent<PlayerController>();
            playerController.Initialize(this);
        }
        
        #endregion

        #region Enemies Manager

        /// <summary>
        /// [재헌] EnemiesSpawnLocations Tag Script를 가진 트랜스폼을 찾아 자신을 제외한 자식들의 Transform (적들의 스폰 위치)를 받아다가 리스트에 저장
        /// </summary>
        private void GetEnemySpawnLocations()
        {
            foreach (var tr in GameObject.FindObjectOfType<EnemiesSpawnLocations>().transform.GetComponentsInChildren<Transform>(false))
            {
                if (tr.GetComponent<EnemiesSpawnLocations>())
                {
                    continue;
                }
                
                spawnLocations.Add(tr);
            }
        }
        
        /// <summary>
        /// [재헌] 적들을 스폰하고 적들을 List에 캐싱하고
        /// 적들이 죽으면 List에서 제거하는 방식
        /// </summary>
        private void EnemiesSpawn()
        {
           
        }

        #endregion

        #region Event Functions

        private void OnPlayerDead()
        {
            // todo.. 게임종료 처리    
        }
        
        #endregion
        
    }
}
