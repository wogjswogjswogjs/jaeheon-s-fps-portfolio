using UnityEngine;

namespace _2.Scripts.Player
{
    public class PlayerValues : MonoBehaviour
    {
        // -- Animator
        internal int horizontalParamHash; // 애니메이터 관련 가로축 값
        internal int verticalParamHash; // 애니메이터 관련 세로축 값
        internal int groundedParamHash; // 애니메이터 지상에 있는가
        
        // -- Collider
        internal Vector3 colliderExtents; // 땅과의 충돌체크를 위한 충돌체 영역.
        
        internal void Initialize()
        {
            SetAnimatorValue();
            SetColliderValue();
        }

        private void SetAnimatorValue()
        {
            horizontalParamHash = Animator.StringToHash(AnimatorKey.Horizontal);
            verticalParamHash = Animator.StringToHash(AnimatorKey.Vertical);
            groundedParamHash = Animator.StringToHash(AnimatorKey.Grounded);
        }

        private void SetColliderValue()
        {
            colliderExtents = GetComponent<Collider>().bounds.extents;
        }

    }
}
