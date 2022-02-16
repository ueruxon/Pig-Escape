using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.UI.Elements
{
    public class BombReloadIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;
        
        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);

        public void SetIndicatorValue(float value) => 
            _indicator.fillAmount = value;
    }
}