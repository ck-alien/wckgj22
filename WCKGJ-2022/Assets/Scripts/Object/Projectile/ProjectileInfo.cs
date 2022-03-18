using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

이 클래스는 발사체의 정보를 담은 ScriptableObject 클래스이다 추후 발사체 추가시, ProjectileManager에 추가하지 않아도
작동하도록 Pooling 데이터의 정보는 이 클래스가 담고있다.

-변수-
1. _resource 변수는 프리팹의 정보를 담고있다.

2. _limit 변수는 resource에 담겨있는 오브젝트를 몇개까지 생성할지 정하는 정수이다.

3. _projectiles 변수는 만들어진 발사체의 총 갯수를 저장하는 list이다.

4. _poolingOn 변수는 현재 활동하고있는 발사체의 list이다.

5. _poolingOff 변수는 현재 활성화 되지 않은 발사체의 list이다.

-함수-
1. Reset함수는 ScriptableObject에 남아있는 데이터를 삭제하여 데이터가 쌓이지 않도록 관리한다.


*/



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
        public GameObject _resource;
        public int _limit;
        public List<Projectile> _projectiles = new List<Projectile>();
        public List<Projectile> _poolingOn = new List<Projectile>();

        public List<Projectile> _poolingOff = new List<Projectile>();

    }
}
