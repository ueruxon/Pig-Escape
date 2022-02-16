using UnityEngine;

namespace Game.Code.UI.Elements
{
    public class DetectionIndicator : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        public void Show() => 
            _canvas.enabled = true;

        public void Hide() => 
            _canvas.enabled = false;
    }
}