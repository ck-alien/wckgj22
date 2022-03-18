using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{
    /// <summary>
    /// 이 클래스는 발사체의 정보를 담은 ScriptableObject 클래스이다.<para/>
    /// 추후 발사체 추가시, ProjectileManager에 추가하지 않아도 작동하도록 Pooling 데이터의 정보는 이 클래스가 담고있다.
    /// </summary>
    [CreateAssetMenu(fileName = "ProjectileInfo", menuName = "ScriptableObj/ProjectileInfo")]
    public class ProjectileInfo : ScriptableObject
    {
        /// <summary>
        /// 프리팹의 정보를 담고있다.
        /// </summary>
        public GameObject _resource;

        /// <summary>
        /// resource에 담겨있는 오브젝트를 몇개까지 생성할지 정하는 정수이다.
        /// </summary>
        public int _limit;

        /// <summary>
        /// 변수는 만들어진 발사체의 총 갯수를 저장하는 list이다.
        /// </summary>
        public List<Projectile> _projectiles = new();

        /// <summary>
        /// 현재 활동하고있는 발사체의 list이다.
        /// </summary>
        public List<Projectile> _poolingOn = new();

        /// <summary>
        /// 현재 활성화 되지 않은 발사체의 list이다.
        /// </summary>
        public List<Projectile> _poolingOff = new();

        /// <summary>
        /// ScriptableObject에 남아있는 데이터를 삭제하여 데이터가 쌓이지 않도록 관리한다.
        /// </summary>
        public void Reset()
        {
            _projectiles.Clear();
            _poolingOn.Clear();
            _poolingOff.Clear();
        }
    }
}
