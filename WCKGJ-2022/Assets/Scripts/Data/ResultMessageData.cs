using System;
using UnityEngine;

namespace EarthIsMine.Data
{
    [Serializable]
    public class ResultMessage
    {
        [SerializeField, Tooltip("해당 점수 이상 획득 시 메시지가 출력됩니다.")]
        private int _score;

        [SerializeField, Tooltip("출력할 메시지")]
        private string _message;

        public int Score => _score;
        public string Message => _message;
    }

    [CreateAssetMenu(fileName = "ResultMessageData", menuName = "Data/ResultMessageData")]
    public class ResultMessageData : ScriptableObject
    {
        [SerializeField, Tooltip("메시지 목록")]
        private ResultMessage[] _messages;

        public ResultMessage[] Messages => _messages;
    }
}
