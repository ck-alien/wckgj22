using UnityEngine;

namespace EarthIsMine
{
    public class UITest : MonoBehaviour
    {
        public void OnClick()
        {
            SceneLoader.Instance.Load("GameScene");
        }
    }
}
