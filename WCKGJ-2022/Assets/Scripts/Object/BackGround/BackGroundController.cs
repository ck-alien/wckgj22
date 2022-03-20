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

        private BackGroundPart[] parts;

        private Transform tr;

        private void Awake()
        {
            tr = GetComponent<Transform>();
            parts = GetComponentsInChildren<BackGroundPart>();
        }

        public void ChangeSprite(Day time)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].ChangeSprite(time);
            }
        }

        private void Update()
        {
            tr.position += new Vector3(0, Speed, 0) * Time.deltaTime;
        }


    }
}
