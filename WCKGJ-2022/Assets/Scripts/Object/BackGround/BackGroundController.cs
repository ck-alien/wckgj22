using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class BackGroundController : MonoBehaviour
    {
        [field: SerializeField]
        public float Speed { get; set; }


        private Transform tr;

        private void Awake()
        {
            tr = GetComponent<Transform>();
        }
        private void Update()
        {
            tr.position += new Vector3(0, Speed, 0) * Time.deltaTime;
        }


    }
}
