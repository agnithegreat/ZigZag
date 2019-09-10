using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public abstract class PoolingObject<T> : MonoBehaviour where T : PoolingObject<T>
    {
        private static T _prefab;
        private static Transform _parent;
        private static Queue<T> _pool = new Queue<T>();

        public static void Init(T prefab, Transform parent, int initialCount = 0)
        {
            _prefab = prefab;
            _parent = parent;
            for (int i = 0; i < initialCount; i++)
            {
                Instantiate(_prefab, _parent).Release();
            }
        }

        public static T GetNew()
        {
            var obj = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(true);
            obj._inPool = false;
            return obj;
        }

        public static void Release(T obj)
        {
            _pool.Enqueue(obj);
            obj.Reset();
            obj.gameObject.SetActive(false);
            obj._inPool = true;
        }
        
        public abstract void Reset();

        public void Release()
        {
            _pool.Enqueue((T) this);
            Reset();
            gameObject.SetActive(false);
        }

        private bool _inPool;
        public bool InPool => _inPool;
    }
}