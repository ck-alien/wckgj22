using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Player
{
    public class PlayerMove : MonoBehaviour
    {
        private Transform tr;
        private PlayerController controller;



        private void Awake()
        {
            if (tr == null)
                tr = GetComponent<Transform>();
            if (controller == null)
                controller = GetComponent<PlayerController>();

        }




        public void MoveOn(float _direction)
        {
            tr.position += new Vector3(tr.position.x + _direction, tr.position.y, tr.position.z);
        }

    }
}
