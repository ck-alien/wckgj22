using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EarthIsMine.Manager;

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

        public void MoveRight(float _direction)
        {
            tr.position = new Vector3(tr.position.x + (_direction * controller.Speed * Time.deltaTime), tr.position.y, tr.position.z);
        }

    }
}
