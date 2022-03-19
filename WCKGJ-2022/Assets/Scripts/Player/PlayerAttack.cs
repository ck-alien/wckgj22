using EarthIsMine.Manager;
using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Player
{
    /// <summary>
    /// 플레이어의 공격을 담당하는 클래스이다. 직접적인 공격 호출과 쿨타임을 계산하는 역할을 한다.
    /// </summary>
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("플레이어가 발사할 발사체의 정보를 모아둔 변수이다.")]
        private ProjectileInfo _info;

        [SerializeField]
        [Tooltip("발사체가 시작하는 위치를 나타낸다.")]
        private Transform _startPos;

        [SerializeField]
        [Tooltip("한번에 발사하는 발사체가 2개 이상일경우, 발사체사이의 간격을 나타낸다.")]
        private float _distance;

        [SerializeField]
        [Tooltip("발사체를 다시 발사하기까지의 시간을 나타낸다.")]
        private float _coolTime;

        /// <summary>
        /// 직접적으로 쿨타임 계산을 하는 변수이다.
        /// </summary>
        private float _curCoolTime;

        private FMOD.Studio.EventInstance instance;

        private void Start()
        {
            _info.Reset();
            _curCoolTime = _coolTime;
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/ShotSFX2D");
        }

        /// <summary>
        /// 쿨타임 업데이트 작업을 실행한다.
        /// </summary>
        private void CoolTimeUpdate()
        {
            if (_curCoolTime > 0)
            {
                _curCoolTime -= Time.deltaTime;
            }
            else
            {
                _curCoolTime = _coolTime;
                ProjectileManager.Instance.CreateProjectile(_info, _startPos.position, Quaternion.identity);
                instance.setVolume(EarthIsMine.System.GameSound.SFXVolume);
                instance.start();

            }
        }

        private void Update()
        {
            CoolTimeUpdate();
        }
    }
}
