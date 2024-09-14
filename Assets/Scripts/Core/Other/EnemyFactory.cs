using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.Entities;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Other
{
    public class EnemyFactory
    {
        private const float SCATTER_RADIUS = 5f;

        private readonly EnemyFactoryData _factoryData;

        public EnemyFactory(EnemyFactoryData factoryData)
        {
            _factoryData = factoryData;

            EventBus.Subscribe<EnemyDeathEvent>(OnEnemyDeathEvent);
        }

        ~EnemyFactory()
        {
            EventBus.Unsubscribe<EnemyDeathEvent>(OnEnemyDeathEvent);
        }

        private void OnEnemyDeathEvent(EnemyDeathEvent evt)
        {
            Debug.LogWarning("EnemySpawned");

            evt.Source.gameObject.SetActive(false);

            var spawnPoints = _factoryData.GetSpawnPoints();
            var groupPoints = _factoryData.GetGroupPoints();

            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var groupPoint = groupPoints[Random.Range(0, groupPoints.Length)];

            SpawnEnemy(evt.Source, spawnPoint, groupPoint);
        }

        private void SpawnEnemy(Enemy enemy, Transform spawnPoint, Transform groupPoint)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;

            var scatterOffset = new Vector3(
                Random.Range(-SCATTER_RADIUS / 2, SCATTER_RADIUS / 2),
                0,
                Random.Range(-SCATTER_RADIUS / 2, SCATTER_RADIUS / 2)
            );

            var scatteredGroupPoint = groupPoint.position + scatterOffset;

            enemy.gameObject.SetActive(true);
            enemy.Initialize();
            enemy.MoveTo(scatteredGroupPoint);
        }
    }
}