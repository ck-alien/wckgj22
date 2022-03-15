using System.Collections;
using Cysharp.Threading.Tasks;
using EarthIsMine.FSM;
using EarthIsMine.Manager;
using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Game
{
    public class StageBehaviour : StateBehaviour
    {
        private Unity.Mathematics.Random _random;

        private void Start()
        {
            _random = new Unity.Mathematics.Random((uint)Random.Range(0, 1000000));
        }

        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(500).ToCoroutine();
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine)
        {
            var randomIdx = _random.NextInt(3);
            Enemy spawnedEnemy;

            using var enemyType = randomIdx switch
            {
                0 => EnemyManager.Instance.SpawnEnemy<EnemyTypeA>(out spawnedEnemy),
                1 => EnemyManager.Instance.SpawnEnemy<EnemyTypeB>(out spawnedEnemy),
                2 => EnemyManager.Instance.SpawnEnemy<EnemyTypeC>(out spawnedEnemy),
                _ => EnemyManager.Instance.SpawnEnemy<EnemyTypeA>(out spawnedEnemy),
            };

            spawnedEnemy.transform.position = new Vector2(0f, 3f);

            while (spawnedEnemy.transform.position.y > -3f)
            {
                var pos = spawnedEnemy.transform.position;
                pos.y -= 3f * Time.deltaTime;
                spawnedEnemy.transform.position = pos;

                yield return null;
            }

            stateMachine.ChangeState(typeof(ReadyBehaviour));
            yield break;
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(500).ToCoroutine();
        }
    }
}

