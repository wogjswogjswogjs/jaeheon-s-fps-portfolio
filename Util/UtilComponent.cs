using UnityEngine;

namespace _2.Scripts.Util
{
    public static class UtilComponent<T> where T : Behaviour
    {
        /// <summary>
        /// [재헌] 컴포넌트를 가져오고 싶은 오브젝트의 트랜스폼을 받아
        /// 그 트랜스폼의 오브젝트가 컴포넌트를 가지고 있는지 판단하고
        /// 있으면 리턴해주고 없으면 추가하고 리턴해줌.
        /// </summary>
        /// <param name="owner"> 컴포넌트를 획득하고 싶은 오브젝트 </param>
        /// <returns></returns>
        public static T GetComponent(Transform owner)
        {
            return owner.GetComponent<T>() ? owner.GetComponent<T>() : owner.gameObject.AddComponent<T>();
        }
    }
}