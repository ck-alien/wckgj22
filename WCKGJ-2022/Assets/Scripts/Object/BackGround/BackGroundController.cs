using UnityEngine;

namespace EarthIsMine.Object
{
    public enum Day
    {
        DayTime, Night
    }


    public class BackGroundController : MonoBehaviour
    {
        [field: SerializeField]
        public float Speed { get; set; }

        // private BackGroundPart[] _parts;

        /*
        private void Awake()
        {
            _parts = GetComponentsInChildren<BackGroundPart>();
        }
        */
        /*
        public void ChangeSprite(Day time)
        {
            for (int i = 0; i < _parts.Length; i++)
            {
                _parts[i].ChangeSprite(time);
            }
        }
        */

        private void Update()
        {
            transform.position += new Vector3(0, Speed, 0) * Time.deltaTime;
        }
    }
}
