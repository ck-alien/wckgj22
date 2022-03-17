using UniRx;
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
            StartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    CanvasGroup.interactable = false;
                    SceneLoader.Instance.Load("GameScene");
                });
        }
    }
}
