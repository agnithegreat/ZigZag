using Components;
using UnityEngine;
using Utils;

namespace Entities
{
    [RequireComponent(typeof(MoveComponent))]
    public class Ball : PoolingObject<Ball>
    {
        public float X;
        public float Y;

        private MoveComponent _moveComponent;
        public MoveComponent MoveComponent => _moveComponent;

        private bool _movesForward;

        private void Awake()
        {
            _moveComponent = GetComponent<MoveComponent>();
        }

        public void Switch()
        {
            if (!_moveComponent.enabled)
            {
                _moveComponent.enabled = true;
            }
            else
            {
                _movesForward = !_movesForward;
                _moveComponent.Direction = _movesForward ? Vector3.forward : Vector3.right;
            }
        }
    
        public void Fall()
        {
            _moveComponent.enabled = false;
            var falling = gameObject.AddComponent<FallingComponent>();
            falling.OnComplete = Release;
        }
    
        private void Update()
        {
            var position = transform.position;
            X = position.x;
            Y = position.z;
        }

        public override void Reset()
        {
            transform.position = new Vector3();
            X = 0;
            Y = 0;
            _movesForward = false;
            _moveComponent.enabled = false;
            _moveComponent.Direction = _movesForward ? Vector3.forward : Vector3.right;
        }
    }
}