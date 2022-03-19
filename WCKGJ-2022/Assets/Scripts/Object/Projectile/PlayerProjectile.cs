using System.Collections;
using EarthIsMine.Manager;
using UnityEngine;


namespace EarthIsMine.Object
{
    public class PlayerProjectile : Projectile
    {

        /*
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                Debug.Log("you hit enemy!");
                ProjectileManager.Instance.PoolingOff(this);
                StopAllCoroutines();
            }
        }
        */

        protected override IEnumerator DurableTimeUpdate()
        {
            yield return new WaitForSeconds(_durableTime);
            ProjectileManager.Instance.PoolingOff(this);
            yield return null;
        }

    }
}
