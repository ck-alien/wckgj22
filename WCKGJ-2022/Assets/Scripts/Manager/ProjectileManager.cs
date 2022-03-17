using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Object;


namespace EarthIsMine.Manager
{
    public class ProjectileManager : Singleton<ProjectileManager>
    {

        public void CreateProjectile(ProjectileInfo info, Vector3 startPos, Quaternion quaternion)
        {
            Projectile tmp;
            if (info._poolingOn.Count < info._limit)
            {
                tmp = Instantiate(info.resource, startPos, quaternion).GetComponent<Projectile>();
                tmp._info = info;
                info._projectiles.Enqueue(tmp);
                info._poolingOn.Enqueue(tmp);
            }
            else
            {
                tmp = info._poolingOff.Dequeue();
                SetActiveObj(tmp, true);
                tmp.transform.position = startPos;
            }
        }
        public void CreateProjectiles(ProjectileInfo info, Vector3 startPos, Quaternion quaternion, float distance)
        {
            Projectile tmp;
            if (info._projectiles.Count < info._limit)
            {
                tmp = Instantiate(info.resource, new Vector3(startPos.x - distance, startPos.y, startPos.z), quaternion).GetComponent<Projectile>();
                tmp._info = info;
                info._projectiles.Enqueue(tmp);
                info._poolingOn.Enqueue(tmp);
            }
            else
            {
                tmp = info._poolingOff.Dequeue();
                SetActiveObj(tmp, true);
                tmp.transform.position = new Vector3(startPos.x - distance, startPos.y, startPos.z);

            }
            if (info._projectiles.Count < info._limit)
            {
                tmp = Instantiate(info.resource, new Vector3(startPos.x + distance, startPos.y, startPos.z), quaternion).GetComponent<Projectile>();
                tmp._info = info;
                info._projectiles.Enqueue(tmp);
                info._poolingOn.Enqueue(tmp);

            }
            else
            {
                tmp = info._poolingOff.Dequeue();
                SetActiveObj(tmp, true);
                tmp.transform.position = new Vector3(startPos.x + distance, startPos.y, startPos.z);

            }
        }

        public void Pooling(Projectile obj, bool value)
        {
            Projectile tmp = obj._info._poolingOn.Dequeue();
            SetActiveObj(tmp, value);

        }

        protected void SetActiveObj(Projectile obj, bool value)
        {
            if (value)
                obj._info._poolingOn.Enqueue(obj);
            else
                obj._info._poolingOff.Enqueue(obj);
            obj.gameObject.SetActive(value);

        }


    }
}
