using System.Collections.Generic;
using UnityEngine;

namespace TRKGeneric
{
    public abstract class GenericObjectPool<T> : MonoSingleton<GenericObjectPool<T>> where T : Component
    {
        [SerializeField]
        private T prefab;

        private Queue<T> objects = new Queue<T>();

        public T Get()
        {
            if (objects.Count == 0)
            {
                AddObjects(1);
            }
            return objects.Dequeue();
        }
        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            objects.Enqueue(objectToReturn);
        }
        private void AddObjects(int num)
        {
            for (int i = 0; i < num; i++)
            {
                T obj = Instantiate(prefab);
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(transform);
                objects.Enqueue(obj);
            }
        }
    }
}
