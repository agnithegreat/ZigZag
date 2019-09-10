using UnityEngine;

namespace Utils
{
    public class CameraController : MonoBehaviour
    {
        public GameController GameController;
    
        public Transform CameraTransform;
    
        private void LateUpdate()
        {
            if (!GameController.Ball) return;
            CameraTransform.position = GameController.Ball.transform.position;
        }
    }
}