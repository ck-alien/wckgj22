using System;
using UnityEngine;

namespace EarthIsMine.Data
{
    [Serializable]
    public class ResultMessage
    {
        [field: SerializeField]
        public int Score { get; private set; }

        [field: SerializeField, Multiline]
        public string Message { get; private set; }
    }

    [CreateAssetMenu(fileName = "ResultMessageData", menuName = "Data/ResultMessageData")]
    public class ResultMessageData : ScriptableObject
    {
        [field: SerializeField]
        public ResultMessage[] Messages { get; private set; }
    }
}
