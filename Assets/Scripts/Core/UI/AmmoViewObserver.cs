using Core.Events;
using Modules.AmmoPanelFeature;
using Modules.EventBusFeature;

namespace Core.UI
{
    public class AmmoViewObserver
    {
        private const string AMMO_TEXT_FORMAT = "<b>{0}</b>/{1}";
        
        private readonly AmmoView _view;

        private int _ammoCount;
        private int _totalAmmoCount;
        private bool _isUnlimited;

        public AmmoViewObserver(AmmoView view, int ammoCount)
        {
            _view = view;
            _ammoCount = ammoCount;
            _totalAmmoCount = ammoCount;

            UpdateViewText();

            EventBus.Subscribe<ShootingEvent>(OnShootingEvent);
            EventBus.Subscribe<ReloadingEvent>(OnReloadingEvent);
            EventBus.Subscribe<ReplenishmentAmmoEvent>(OnReplenishmentAmmoEvent);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
        }

        ~AmmoViewObserver()
        {
            EventBus.Unsubscribe<ShootingEvent>(OnShootingEvent);
            EventBus.Unsubscribe<ReloadingEvent>(OnReloadingEvent);
            EventBus.Unsubscribe<ReplenishmentAmmoEvent>(OnReplenishmentAmmoEvent);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
        }
        
        private void OnShootingEvent(ShootingEvent evt)
        {
            _ammoCount -= 1;

            UpdateViewText();
        }

        private void OnReloadingEvent(ReloadingEvent evt)
        {
            _totalAmmoCount -= evt.Ammo;
            _ammoCount += evt.Ammo;

            UpdateViewText();
        }
        
        private void OnReplenishmentAmmoEvent(ReplenishmentAmmoEvent evt)
        {
            _totalAmmoCount += evt.Ammo;
            
            UpdateViewText();
        }
        
        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            _ammoCount = evt.Data.ClipAmmoCount;
            _totalAmmoCount = evt.Data.TotalAmmoCount;
            _isUnlimited = evt.Data.Config.IsUnlimited;
            
            UpdateViewText();
        }

        private void UpdateViewText()
        {
            var text = _isUnlimited 
                ? string.Format(AMMO_TEXT_FORMAT, _ammoCount, "unlimited") 
                : string.Format(AMMO_TEXT_FORMAT, _ammoCount, _totalAmmoCount);
                
            _view.SetAmmoText(text);
        }
    }
}