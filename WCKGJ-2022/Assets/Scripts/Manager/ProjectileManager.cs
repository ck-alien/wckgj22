using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public class ProjectileManager : Singleton<ProjectileManager>
    {
        List<Projectile> projectiles = new List<Projectile>();

        public void CreateProjectile(GameObject resource, Vector3 startPos, Quaternion quaternion)
        {
            Instantiate(resource, startPos, quaternion);
        }
        public void CreateProjectiles(GameObject resource, Vector3 startPos, Quaternion quaternion, float distance)
        {
            Instantiate(resource, new Vector3(startPos.x - distance, startPos.y, startPos.z), quaternion);
            Instantiate(resource, new Vector3(startPos.x + distance, startPos.y, startPos.z), quaternion);
        }



        private void Pooling(GameObject obj, bool value)
        {

        }


    }
}
