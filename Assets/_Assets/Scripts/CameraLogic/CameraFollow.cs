using UnityEngine;

namespace _Assets.Scripts.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private Transform _following;

        private void Update()
        {
            if (_following == null)
                return;

            transform.position = _following.position + offset;
        }

        public void Follow(GameObject objectToFollow) =>
            _following = objectToFollow.transform;
    }
}