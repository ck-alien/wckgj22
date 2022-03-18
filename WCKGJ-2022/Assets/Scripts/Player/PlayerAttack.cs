using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Manager;
using EarthIsMine.Object;

/*
이 클래스는 플레이어의 공격을 담당하는 클래스이다. 직접적인 공격 호출과 쿨타임을 계산하는 역할을 한다.

- 변수-
1. _info 변수는 플레이어가 발사할 발사체의 정보를 모아둔 변수이다. 자세한건 Projectileinfo 클래스를 확인

2. _startPos 변수는 발사체가 시작하는 위치를 나타낸다.

3. _distance 변수는 한번에 발사하는 발사체가 2개 이상일경우, 발사체사이의 간격을 나타낸다.

4. _coolTime 변수는 발사체를 다시 발사하기까지의 시간을 나타낸다.

5. _curCoolTime 변수는 직접적으로 쿨타임 계산을 하는 변수이다.

-함수-
1. CoolTimeUpdate 함수는 쿨타임 업데이트 작업을 실행한다.

*/





namespace EarthIsMine.Player
{
    public class PlayerAttack : MonoBehaviour
    {

        [SerializeField] private ProjectileInfo _info;


        [SerializeField] private Transform _startPos;

        [SerializeField] private float distance;

        [SerializeField] private float _coolTime;

        private float _curCoolTime;
        private void Start()
        {
            _info.Reset();
        }
        private void CoolTimeUpdate()
        {
            if (_curCoolTime > 0)
            {
                _curCoolTime -= Time.deltaTime;
            }
            else
            {
                _curCoolTime = _coolTime;
                ProjectileManager.Instance.CreateProjectiles(_info, _startPos.position, Quaternion.identity, distance);
            }
        }

        private void Update()
        {
            CoolTimeUpdate();
        }





    }
}
