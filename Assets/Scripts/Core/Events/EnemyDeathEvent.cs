using Core.Entities;

namespace Core.Events
{
    public struct EnemyDeathEvent
    {
        public Enemy Source;
    }
}