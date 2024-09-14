namespace Core.Events
{
    public struct ReloadingEvent
    {
        public int Ammo;
    }

    public struct ReplenishmentAmmoEvent
    {
        //public int WeaponIndex;
        public int Ammo;
    }

    public struct ReloadingRequest
    {
        //public int WeaponIndex;
        public int Ammo;
    }
}