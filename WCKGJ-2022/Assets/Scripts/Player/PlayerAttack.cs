using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Manager;
using EarthIsMine.Object;

namespace EarthIsMine.Player
{
    public class PlayerAttack : MonoBehaviour
    {

        public ProjectileInfo _info;

        private void Start()
        {
            _info.Reset();
        }

        [SerializeField] private Transform _startPos;

        [SerializeField] private float distance;

        public float _coolTime;

        private float _curCoolTime;
        private void CoolTimeUpdate()
        {
            if (_curCoolTime > 0)
            {
                _curCoolTime -= Time.deltaTime;
            }
            else
            {
                _curCoolTime = _coolTime;
                ProjectileManager.Instance.CreateProjectiles(_info, _startPos.position, Quaternion.identity, distance);
            }
        }

        private void Update()
        {
            CoolTimeUpdate();
        }





    }
}
