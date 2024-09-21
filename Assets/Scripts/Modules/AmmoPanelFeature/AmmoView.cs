using TMPro;
using UnityEngine;

namespace Modules.AmmoPanelFeature
{
    public class AmmoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void SetAmmoText(string ammoText)
        {
            _text.text = ammoText;
        }   
    }
}