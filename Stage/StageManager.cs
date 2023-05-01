using System.Collections.Generic;
using UnityEngine;

namespace _2.Scripts.Stage
{
    public class StageManager : MonoBehaviour
    {
        
        #region Camera

        private ThirdPersonCamera camera;
        
        #endregion


        #region Player

        [SerializeField] private Transform playerSpawnLocation;
        
        private const string playerCharacterPath = "Player/Character";
        private PlayerData.PlayerData playerData;
        
        
        #endregion
        
        
        #region Eenmy

        private List<StateController> enemies;
        
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
            CameraSetting();
            PlayerSetting();       
        }
        
            
        /// <summary>
        /// [재헌] 카메라 세팅에 필요한 초기화 함수를 실행 합니다.
        /// </summary>
        private void CameraSetting()
        {
            camera.Initialize();    
        }

        private void GetEnemySpawnLocations()
        {
            
        }
        
        /// <summary>
        /// [재헌] PlayerData에서 프리팹을 가져와 플레이어를 스폰하고
        /// 플레이어 매니저가 플레이어를 컨트롤하는 스크립트를 캐싱함.
        /// </summary>
        private void PlayerSetting()
        {
            
            Resources<PlayerData.PlayerData>.Load(playerCharacterPath + nameof(playerData));
        }
        

        /// <summary>
        /// [재헌] 적들을 스폰하고 적들을 List에 캐싱하고
        /// 적들이 죽으면 List에서 제거하는 방식
        /// </summary>
        private void EnemiesSpawn()
        {
            
        }


        #region Event Functions

        private void OnPlayerDead()
        {
            // todo.. 게임종료 처리    
        }
        
        #endregion
        
    }
}
