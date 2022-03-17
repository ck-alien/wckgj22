using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{

    [CreateAssetMenu(fileName = "ProjectileInfo", menuName = "ScriptableObj/ProjectileInfo")]
    public class ProjectileInfo : ScriptableObject
    {
        public GameObject resource;
        public int _limit;

        public Queue<Projectile> _projectiles = new Queue<Projectile>();
        public Queue<Projectile> _poolingOn = new Queue<Projectile>();

        public Queue<Projectile> _poolingOff = new Queue<Projectile>();

    }
}
