using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class BackGroundPart : MonoBehaviour
    {
        private Transform tr;

        public Vector3 _startPos;

        public float _limitY;

        private void Awake()
        {
            tr = GetComponent<Transform>();
        }


        private void Update()
        {
            if (tr.position.y < _limitY)
            {
                tr.position = _startPos;
            }
        }

    }
}
