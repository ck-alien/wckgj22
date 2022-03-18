using UnityEngine.SceneManagement;

namespace EarthIsMine.Manager
{
    public class TitleManager : Singleton<TitleManager>
    {
        protected override void Awake()
        {
            base.Awake();
            SceneManager.LoadSceneAsync("BackgroundScene", LoadSceneMode.Additive);
        }
    }
}
