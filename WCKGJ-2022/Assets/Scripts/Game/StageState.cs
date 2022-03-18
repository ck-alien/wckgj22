using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using EarthIsMine.FSM;
using EarthIsMine.Manager;
using EarthIsMine.Object;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Game
{
    public class StageState : StateBehaviour
    {
        private int _idx = 0;

        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            var wait = UniTask.Delay(500).ToObservable().ToYieldInstruction();
            while (!wait.IsDone)
            {
                yield return null;
            }
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine, CancellationToken cancellation)
        {
#pragma warning disable IDE0018 // Inline variable declaration
            Enemy spawnedEnemy;
#pragma warning restore IDE0018 // Inline variable declaration
            using var enemyType = _idx switch
            {
                0 => EnemyManager.Instance.SpawnEnemy<EnemyTypeA>(out spawnedEnemy),
                1 => EnemyManager.Instance.SpawnEnemy<EnemyTypeB>(out spawnedEnemy),
                2 => EnemyManager.Instance.SpawnEnemy<EnemyTypeC>(out spawnedEnemy),
                _ => EnemyManager.Instance.SpawnEnemy<EnemyTypeA>(out spawnedEnemy),
            };
            _idx = _idx < 2 ? _idx + 1 : 0;

            spawnedEnemy.transform.position = new Vector2(0f, 3f);

            while (!cancellation.IsCancellationRequested && spawnedEnemy.transform.position.y > -3f)
            {
                var pos = spawnedEnemy.transform.position;
                pos.y -= 3f * Time.deltaTime;
                spawnedEnemy.transform.position = pos;

                yield return null;
            }

            if (cancellation.IsCancellationRequested)
            {
                yield break;
            }

            stateMachine.ChangeState(typeof(ReadyState));
            yield break;
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            var wait = UniTask.Delay(500).ToObservable().ToYieldInstruction();
            while (!wait.IsDone)
            {
                yield return null;
            }
        }
    }
}

