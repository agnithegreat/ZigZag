using UnityEngine;

namespace Components
{
    public class MoveComponent : MonoBehaviour
    {
        public Vector3 Direction;
        public float Speed;
        
        private void Update()
        {
            transform.position += Speed * Time.deltaTime * Direction;
        }
    }
}