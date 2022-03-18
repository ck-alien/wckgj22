using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Object;

/*

이 클래스는 플레이어 혹은 몬스터가 만들어내는 모든 발사체를 관리하는 Manager 클래스이다.

-함수-
1. CreateProjectile 함수는 만들어내려는 발사체가 1개일때 호출하는 함수로, 만일 만들어내려는 발사체의 수가 한계 이하면 생성,
아니면 Pooling 시스템을 활용한다.

2. CreateProjectiles 함수는 만들어내려는 발사체가 2개일때 호출하는 함수로, 만일 만들어내려는 발사체의 수가 한계 이하면 생성,
아니면 Pooling 시스템을 활용한다.

3. void SetActiveObj 함수는 풀링시스템을 실행하는 함수이다. 첫번째 인자로 풀링하려는 발사체를,
두번째는 활성화시킬지, 비활성화시킬지 여부를 결정하는 bool 변수이다.

4. Projectile SetActiveObj 함수는 풀링시스템을 실행하는 함수이다. 첫번째 인자로 풀링하려는 발사체의 info를,
두번째는 활성화시킬지, 비활성화시킬지 여부를 결정하는 bool 변수이다.(주로 활성화를 목적으로 실행)

5. PoolingOff 함수는 비활성화 시키려는 발사체를 받아 void SetActiveObj 함수를 실행시킨다.


*/





namespace EarthIsMine.Manager
{
    public class ProjectileManager : Singleton<ProjectileManager>
    {

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
        public void PoolingOff(Projectile obj)
        {
            SetActiveObj(obj, false);
        }

    }
}
