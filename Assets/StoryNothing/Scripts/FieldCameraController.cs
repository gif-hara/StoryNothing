using Unity.Cinemachine;
using UnityEngine;

namespace StoryNothing
{
    public class FieldCameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera defaultCinemachineCamera;

        public void SetDefaultTrackingTarget(Transform target)
        {
            defaultCinemachineCamera.Target.TrackingTarget = target;
        }
    }
}
