using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI.Util
{
    [RequireComponent(typeof(Button))]
    public class PlaySFXOnClick : MonoBehaviour
    {
        [SerializeField]
        private FMODUnity.EventReference _sfx;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(PlaySFX);
        }

        private void PlaySFX()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_sfx);
        }
    }
}
