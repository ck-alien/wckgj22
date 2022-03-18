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
                info._projectiles.Add(tmp);
                info._poolingOn.Add(tmp);
            }
            else
            {
                tmp = SetActiveObj(info, true);
                tmp.transform.position = startPos;
            }
        }
        public void CreateProjectiles(ProjectileInfo info, Vector3 startPos, Quaternion quaternion, float distance)
        {
            Projectile tmp;
            Debug.Log(info._poolingOn.Count);
            Debug.Log(info._projectiles.Count);
            if (info._projectiles.Count < info._limit)
            {
                tmp = Instantiate(info.resource, new Vector3(startPos.x - distance, startPos.y, startPos.z), quaternion).GetComponent<Projectile>();
                tmp._info = info;
                info._projectiles.Add(tmp);
                info._poolingOn.Add(tmp);
            }
            else
            {
                tmp = SetActiveObj(info, true);
                tmp.transform.position = new Vector3(startPos.x - distance, startPos.y, startPos.z);

            }
            if (info._projectiles.Count < info._limit)
            {
                tmp = Instantiate(info.resource, new Vector3(startPos.x + distance, startPos.y, startPos.z), quaternion).GetComponent<Projectile>();
                tmp._info = info;
                info._projectiles.Add(tmp);
                info._poolingOn.Add(tmp);

            }
            else
            {
                tmp = SetActiveObj(info, true);
                tmp.transform.position = new Vector3(startPos.x + distance, startPos.y, startPos.z);

            }
        }

        public void PoolingOff(Projectile obj)
        {
            SetActiveObj(obj, false);
        }


        protected void SetActiveObj(Projectile obj, bool value)
        {
            if (value)
            {
                obj._info._poolingOff.Remove(obj);
                obj._info._poolingOn.Add(obj);

            }
            else
            {
                obj._info._poolingOn.Remove(obj);
                obj._info._poolingOff.Add(obj);
            }
            obj.gameObject.SetActive(value);

        }
        protected Projectile SetActiveObj(ProjectileInfo info, bool value)
        {
            Projectile tmp;
            if (value)
            {
                //Debug.Log(info._poolingOff.Count);
                tmp = info._poolingOff[0];

                info._poolingOff.Remove(tmp);
                info._poolingOn.Add(tmp);
            }
            else
            {
                tmp = info._poolingOn[0];
                info._poolingOn.Remove(tmp);
                info._poolingOff.Add(tmp);
            }
            tmp.gameObject.SetActive(value);
            return tmp;
        }
    }
}
