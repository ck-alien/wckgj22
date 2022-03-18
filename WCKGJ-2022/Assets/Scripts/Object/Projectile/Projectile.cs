using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EarthIsMine.Object
{
    public class Projectile : MonoBehaviour
    {
        public ProjectileInfo _info;
        [SerializeField] protected float _speed;

        [SerializeField] protected float _durableTime;

        protected void OnEnable()
        {
            StartCoroutine(DurableTimeUpdate());
        }

        private void Update()
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }


        protected virtual IEnumerator DurableTimeUpdate()
        {
            yield return null;

        }

    }
}
