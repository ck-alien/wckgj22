using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Manager
{
    /// <summary>
    /// 플레이어 혹은 몬스터가 만들어내는 모든 발사체를 관리하는 Manager 클래스이다.
    /// </summary>
    public class ProjectileManager : Singleton<ProjectileManager>
    {
        // TODO: EnemyManager처럼 직접 LinkedList 기반으로 관리 가능하게 변경

        [field: SerializeField]
        public ProjectileInfo ProjectileInfo { get; private set; }

        /// <summary>
        /// 만들어내려는 발사체가 1개일때 호출하는 함수로, 
        /// 만일 만들어내려는 발사체의 수가 한계 이하면 생성, 아니면 Pooling 시스템을 활용한다.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="startPos"></param>
        /// <param name="quaternion"></param>
        public void CreateProjectile(ProjectileInfo info, Vector3 startPos, Quaternion quaternion)
        {
            Projectile tmp;
            if (info._poolingOn.Count < info._limit)
            {
                tmp = Instantiate(info._resource, startPos, quaternion).GetComponent<Projectile>();
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

        /// <summary>
        /// 만들어내려는 발사체가 2개일때 호출하는 함수로,
        /// 만일 만들어내려는 발사체의 수가 한계 이하면 생성, 아니면 Pooling 시스템을 활용한다.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="startPos"></param>
        /// <param name="quaternion"></param>
        /// <param name="distance"></param>
        public void CreateProjectiles(ProjectileInfo info, Vector3 startPos, Quaternion quaternion, float distance)
        {
            Projectile tmp;
            if (info._projectiles.Count < info._limit)
            {
                tmp = Instantiate(info._resource, new Vector3(startPos.x - distance, startPos.y, startPos.z), quaternion).GetComponent<Projectile>();
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
                tmp = Instantiate(info._resource, new Vector3(startPos.x + distance, startPos.y, startPos.z), quaternion).GetComponent<Projectile>();
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

        /// <summary>
        /// 풀링시스템을 실행하는 함수이다.
        /// </summary>
        /// <param name="obj">풀링하려는 발사체</param>
        /// <param name="value">활성화시킬지, 비활성화시킬지 여부를 결정</param>
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

        /// <summary>
        /// 풀링시스템을 실행하는 함수이다.
        /// </summary>
        /// <param name="info">풀링하려는 발사체 정보</param>
        /// <param name="value">활성화시킬지, 비활성화시킬지 여부를 결정</param>
        /// <returns></returns>
        protected Projectile SetActiveObj(ProjectileInfo info, bool value)
        {
            Projectile tmp;
            if (value)
            {
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

        /// <summary>
        /// 비활성화 시키려는 발사체를 받아 void SetActiveObj 함수를 실행시킨다.
        /// </summary>
        /// <param name="obj"></param>
        public void PoolingOff(Projectile obj)
        {
            SetActiveObj(obj, false);
        }

    }
}
