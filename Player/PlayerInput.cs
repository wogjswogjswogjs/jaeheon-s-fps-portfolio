using UnityEngine;


    /// <summary>
    /// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
    /// 감지된 입력값을 PlayerController 사용.
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        private const string verticalAxisName = "Vertical";
        private const string horizontalAxisName = "Horizontal";
        private const string runButtonName = "Run";
        public const string jumpButtonName = "Jump";
        private const string aimButtonName = "Aim";
        public const string shoulderButtonName = "Aim Shoulder";
        public const string shootButtonName = "Fire1";
        public const string pickButtonName = "Pick";
        public const string weaponChangeButtonName = "Change";
        public const string reloadButtonName = "Reload";
        public const string dropButtonName = "Drop";

        public float vertical { get; private set; }
        public float horizontal { get; private set; }
        private bool aim { get; set; }
        public bool run { get; private set; }

    
        public void OnInputUpdate()
        {
            // 추후 게임매니저 구현 후, 게임오버 상태일때는 입력을 받지못하게 구현해야함.
            /*if (GameManager.instance != null && GameManager.instance.isGameOver)
        {
            return;
        }*/
        
            vertical = Input.GetAxis(verticalAxisName);
            horizontal = Input.GetAxis(horizontalAxisName);
            aim = Input.GetButton(aimButtonName);
            run = Input.GetButton(runButtonName);
        }
    }

