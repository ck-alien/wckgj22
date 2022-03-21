using System;
using UnityEngine;

namespace EarthIsMine.Pool
{
    public class PoolingItemGUID : MonoBehaviour
    {
        [SerializeField]
        private string _guid;

        public Guid GUID { get; private set; }

        private void Awake()
        {
            GUID = Guid.NewGuid();
            _guid = GUID.ToString();
        }
    }
}
