using System.Collections.Generic;
using UnityEngine;


    public class PlayerComponents : MonoBehaviour
    {
        // -- 스테이지 매니저
        private StageManager stageManager;
        
        // -- 카메라
        public ThirdPersonCamera thirdPersonCameraScript;
        
        // -- 컴포넌트
        internal Transform playerTransform;
        internal Animator playerAnimator;
        internal Rigidbody playerRigidbody;
    
        // -- Behaviour
        internal List<BaseBehaviour> behaviours = new List<BaseBehaviour>();

        // -- Health
        internal PlayerHealth playerHealth;
        public void Initialize(StageManager sm)
        {
            SetStageManager(sm);
            SetCamera();

            GetComponents();
            GetBehaviour();

            Initialize_Behaviour();
        }

        private void SetStageManager(StageManager sm)
        {
            stageManager = sm;
        }
        
        private void SetCamera()
        {
            if (Camera.main != null)
            {
                thirdPersonCameraScript = Camera.main.GetComponent<ThirdPersonCamera>();
            }
        }

        private void GetComponents()
        {
            playerTransform = GetComponent<Transform>();
            playerAnimator = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.enabled = true;
        }

        private void GetBehaviour()
        {
            foreach (var behaviour in GetComponents<BaseBehaviour>())
            {
                behaviours.Add(behaviour);
            }
        }

        private void Initialize_Behaviour()
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.Initialize();
            }
        }
    }

