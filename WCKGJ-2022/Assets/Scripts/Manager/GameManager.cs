using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public enum StageType
    {
        Day, Night
    }

    public class GameManager : Singleton<GameManager>
    {
        public ReactiveProperty<uint> Score { get; set; }

        public ReactiveProperty<StageType> CurrentStage { get; private set; }

        private void Start()
        {
            Observable
                .FromMicroCoroutine<uint>((observer) => AutoScoring(observer))
                .Subscribe(addScore => Score.Value += addScore)
                .AddTo(gameObject);

            Observable
                .FromCoroutine<StageType>((observer) => Run(observer));
        }

        private IEnumerator AutoScoring(IObserver<uint> observer)
        {
            float deltaTime = 0f;

            while (true)
            {
                deltaTime += Time.deltaTime;
                if (deltaTime >= 10.0f)
                {
                    var integerPart = Mathf.Floor(deltaTime);
                    deltaTime -= integerPart;
                    observer.OnNext(50);
                }
                yield return null;
            }
        }

        private IEnumerator Run(IObserver<StageType> observer)
        {
            yield break;
        }
    }
}
