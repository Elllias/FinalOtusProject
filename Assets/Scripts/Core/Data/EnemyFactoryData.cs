using Core.Entities;
using UnityEngine;

namespace Core.Data
{
    public class EnemyFactoryData : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Transform[] _groupPoints;

        public Transform GetParentTransform()
        {
            return _parent;
        }
        
        public Transform[] GetSpawnPoints()
        {
            return _spawnPoints;
        }
        
        public Transform[] GetGroupPoints()
        {
            return _groupPoints;
        }
    }
}