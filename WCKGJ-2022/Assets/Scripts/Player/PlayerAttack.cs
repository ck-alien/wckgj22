using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Manager;


namespace EarthIsMine.Player
{
    public class PlayerAttack : MonoBehaviour
    {

        private GameObject projectile;

        [SerializeField] private Transform _startPos;

        [SerializeField] private float distance;

        public float _coolTime;

        private float _curCoolTime;

        private void Awake()
        {
            projectile = Resources.Load<GameObject>("Prefab/Projectile");
        }
        private void CoolTimeUpdate()
        {
            if (_curCoolTime > 0)
            {
                _curCoolTime -= Time.deltaTime;
            }
            else
            {
                _curCoolTime = _coolTime;
                ProjectileManager.Instance.CreateProjectiles(projectile, _startPos.position, Quaternion.identity, distance);
            }
        }

        private void Update()
        {
            CoolTimeUpdate();
        }





    }
}
