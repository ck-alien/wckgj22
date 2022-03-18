using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Manager;


namespace EarthIsMine.Object
{
    public class PlayerProjectile : Projectile
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                Debug.Log("you hit enemy!");
            }
        }


        protected override IEnumerator DurableTimeUpdate()
        {
            yield return new WaitForSeconds(_durableTime);
            ProjectileManager.Instance.Pooling(this, false);
            yield return null;
        }

    }
}
