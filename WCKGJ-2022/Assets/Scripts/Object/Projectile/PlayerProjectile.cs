using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Manager;


namespace EarthIsMine.Object
{
    public class PlayerProjectile : Projectile
    {



        protected override IEnumerator DurableTimeUpdate()
        {
            yield return new WaitForSeconds(_durableTime);
            ProjectileManager.Instance.Pooling(this, false);
            yield return null;
        }

    }
}
