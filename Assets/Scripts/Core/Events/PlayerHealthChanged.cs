namespace Core.Events
{
    public struct PlayerHealthChanged
    {
        public readonly float Health;

        public PlayerHealthChanged(float health)
        {
            Health = health;
        }
    }
}