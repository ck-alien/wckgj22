using UnityEngine;

namespace EarthIsMine.Pool
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePoolingItem : MonoBehaviour
    {
        private void Start()
        {
            var system = GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            if (gameObject.TryGetComponent<ReturnToPool>(out var component))
            {
                component.Return();
            }
        }
    }
}
