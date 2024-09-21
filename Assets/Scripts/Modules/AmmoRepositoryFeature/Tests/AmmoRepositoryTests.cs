using System.Collections.Generic;
using NUnit.Framework;

namespace Modules.AmmoRepositoryFeature.Tests
{
    public class AmmoRepositoryTests
    {
        private AmmoRepository _ammoRepository;
        private Dictionary<int, int> _clipAmmoRepository;
        private Dictionary<int, int> _totalAmmoRepository;

        [SetUp]
        public void SetUp()
        {
            _clipAmmoRepository = new Dictionary<int, int>
            {
                { 1, 10 },
                { 2, 5 }
            };

            _totalAmmoRepository = new Dictionary<int, int>
            {
                { 1, 50 },
                { 2, 30 }
            };

            _ammoRepository = new AmmoRepository(_clipAmmoRepository, _totalAmmoRepository);
        }

        [Test]
        public void ReleaseAmmoTest_EnoughAmmo()
        {
            int weaponId = 1;
            int ammoToRelease = 20;

            int releasedAmmo = _ammoRepository.ReleaseAmmo(weaponId, ammoToRelease);

            Assert.AreEqual(ammoToRelease, releasedAmmo);
            Assert.AreEqual(30, _totalAmmoRepository[weaponId]);
            Assert.AreEqual(30, _clipAmmoRepository[weaponId]);
        }

        [Test]
        public void ReleaseAmmoTest_NotEnoughAmmo()
        {
            int weaponId = 2;
            int ammoToRelease = 40;

            int releasedAmmo = _ammoRepository.ReleaseAmmo(weaponId, ammoToRelease);

            Assert.AreEqual(30, releasedAmmo);
            Assert.AreEqual(0, _totalAmmoRepository[weaponId]);
            Assert.AreEqual(35, _clipAmmoRepository[weaponId]);
        }

        [Test]
        public void ReleaseAmmoTest_NotContainsId()
        {
            int weaponId = 3;
            int ammoToRelease = 10;

            int releasedAmmo = _ammoRepository.ReleaseAmmo(weaponId, ammoToRelease);

            Assert.AreEqual(0, releasedAmmo);
            Assert.IsFalse(_totalAmmoRepository.ContainsKey(weaponId));
            Assert.IsFalse(_clipAmmoRepository.ContainsKey(weaponId));
        }

        [Test]
        public void AddTotalAmmoTest_ValidAmount()
        {
            int weaponId = 1;
            int ammoToAdd = 20;

            _ammoRepository.AddTotalAmmo(weaponId, ammoToAdd);

            Assert.AreEqual(70, _totalAmmoRepository[weaponId]);
        }

        [Test]
        public void AddTotalAmmoTest_InvalidAmount()
        {
            int weaponId = 1;
            int ammoToAdd = -10;

            _ammoRepository.AddTotalAmmo(weaponId, ammoToAdd);

            Assert.AreEqual(50, _totalAmmoRepository[weaponId]);
        }

        [Test]
        public void RemoveClipAmmoTest_ValidAmount()
        {
            int weaponId = 1;
            int ammoToRemove = 5;

            _ammoRepository.RemoveClipAmmo(weaponId, ammoToRemove);

            Assert.AreEqual(5, _clipAmmoRepository[weaponId]);
        }

        [Test]
        public void RemoveClipAmmoTest_InvalidAmount()
        {
            int weaponId = 1;
            int ammoToRemove = -5;

            _ammoRepository.RemoveClipAmmo(weaponId, ammoToRemove);

            Assert.AreEqual(10, _clipAmmoRepository[weaponId]);
        }

        [Test]
        public void GetClipAmmoCountTest()
        {
            int weaponId = 1;

            int clipAmmoCount = _ammoRepository.GetClipAmmoCount(weaponId);

            Assert.AreEqual(10, clipAmmoCount);
        }

        [Test]
        public void GetTotalAmmoCountTest()
        {
            int weaponId = 1;

            int totalAmmoCount = _ammoRepository.GetTotalAmmoCount(weaponId);

            Assert.AreEqual(50, totalAmmoCount);
        }
    }
}
