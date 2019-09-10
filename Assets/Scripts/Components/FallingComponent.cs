using System;
using UnityEngine;

namespace Components
{
    public class FallingComponent : MonoBehaviour
    {
        public float FallHeight = -20;
        public float FallSpeed = 9.8f;
        public Action OnComplete;
        
        private void Update()
        {
            if (transform.position.y < FallHeight)
            {
                Destroy(this);
                OnComplete?.Invoke();
                return;
            }
        
            transform.position += Time.deltaTime * FallSpeed * Vector3.down;
        }
    }
}