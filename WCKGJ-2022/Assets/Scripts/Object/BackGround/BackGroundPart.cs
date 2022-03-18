using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class BackGroundPart : MonoBehaviour
    {
        private Transform tr;

        [field: SerializeField]
        private Vector3 _startPos;

        [field: SerializeField]
        private float _limitY;

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
