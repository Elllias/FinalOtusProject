namespace Core.Events
{
    public struct PlayerHealthChanged
    {
        public readonly int Health;

        public PlayerHealthChanged(int health)
        {
            Health = health;
        }
    }
}