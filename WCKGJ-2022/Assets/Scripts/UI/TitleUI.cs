using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI
{
    public class TitleUI : Presenter
    {
        [field: SerializeField]
        public Button StartButton { get; private set; }

        private void Start()
        {
        }
    }
}
