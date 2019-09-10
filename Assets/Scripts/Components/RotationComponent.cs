using UnityEngine;

namespace Components
{
    public class RotationComponent : MonoBehaviour
    {
        public float RotationSpeed = 90;
        
        private void Update()
        {
            transform.eulerAngles += new Vector3(0, Time.deltaTime * RotationSpeed, 0);
        }
    }
}