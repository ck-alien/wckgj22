using System;
using System.Collections.Generic;
using EarthIsMine.Pool;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        [Serializable]
        public class Particle
        {
            [SerializeField, Tooltip("호출 시 사용할 고유 이름")]
            private string _uniqueId;

            [SerializeField, Tooltip("파티클 프리팹")]
            private ParticleSystem _particlePrefab;

            public string UniqueId => _uniqueId;
            public ParticleSystem ParticlePrefab => _particlePrefab;
        }

        [SerializeField]
        private Particle[] _particles;

        private readonly Dictionary<string, IGameObjectPool> _pools = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (var particle in _particles)
            {
                var pool = new GameObjectPool(particle.ParticlePrefab.gameObject);
                Debug.Assert(_pools.TryAdd(particle.UniqueId, pool), $"{particle.UniqueId}와 중복되는 파티클이 이미 존재합니다.");
            }
        }

        public void Play(string id, Vector3 position)
        {
            if (!_pools.TryGetValue(id, out var pool))
            {
                throw new ArgumentException($"잘못된 파티클 이름입니다! ({id})", nameof(id));
            }
            var particle = pool.Take();
            particle.transform.position = position;
        }
    }
}
