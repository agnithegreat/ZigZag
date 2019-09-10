using System;
using UnityEngine;

namespace Components
{
    public class FlyingComponent : MonoBehaviour
    {
        public float FlyHeight = 4;
        public float FlySpeed = 6;
        public Action OnComplete;
        
        private void Update()
        {
            if (transform.position.y > FlyHeight)
            {
                Destroy(this);
                OnComplete?.Invoke();
                return;
            }
        
            transform.position += Time.deltaTime * FlySpeed * Vector3.up;
        }
    }
}