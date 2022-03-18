using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{

    [CreateAssetMenu(fileName = "ProjectileInfo", menuName = "ScriptableObj/ProjectileInfo")]
    public class ProjectileInfo : ScriptableObject
    {



        public void Reset()
        {
            _projectiles.Clear();
            _poolingOn.Clear();
            _poolingOff.Clear();
        }
        public List<Projectile> _projectiles = new List<Projectile>();
        public List<Projectile> _poolingOn = new List<Projectile>();

        public List<Projectile> _poolingOff = new List<Projectile>();
        public GameObject resource;
        public int _limit;

    }
}
