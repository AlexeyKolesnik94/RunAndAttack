using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pool
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private PoolObject prefab;
        
        [Space(10)] 
        [SerializeField] private Transform container;

        [SerializeField] private int maxCapacity;
        [SerializeField] private int minCapacity;

        [Space(10)] 
        [SerializeField] private bool isExpand;

        private List<GameObject> _pool;

        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void OnValidate()
        {
            if (isExpand)
            {
                maxCapacity = Int32.MaxValue;
            }
        }

        private void Start()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            _pool = new List<GameObject>(minCapacity);

            for (int i = 0; i < minCapacity; i++)
            {
                CreateElement();
            }
        }

        private GameObject CreateElement(bool isActiveByDefault = false)
        {
            var createObj = _diContainer.InstantiatePrefab(prefab,container);
            createObj.gameObject.SetActive(isActiveByDefault);
            
            _pool.Add(createObj);

            return createObj;
        }

        public bool TryGetElement(out GameObject element)
        {
            foreach (var i in _pool)
            {
                if (i.gameObject.activeInHierarchy) continue;
                element = i;
                i.gameObject.SetActive(true);
                return true;
            }

            element = null;
            return true;
        }
        
        public GameObject GetFreeElement(Vector3 position, Quaternion rotation)
        {
            var element = GetFreeElement();
            element.transform.position = position;
            element.transform.rotation = rotation;
            return element;
        }

        public GameObject GetFreeElement() 
        {
            if (TryGetElement(out var element))
            {
                return element;
            }

            if (isExpand)
            {
                return CreateElement(true);
            }

            if (_pool.Count < maxCapacity)
            {
                return CreateElement(true);
            }

            throw new Exception("Pool is Over");
        }

        
    }
}