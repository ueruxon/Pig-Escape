using Shapes;
using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public class DetectionZone : MonoBehaviour
    {
        [SerializeField] private Disc _disc;

        public void Init(float radius, float angleDegrees)
        {
            float rad = angleDegrees * Mathf.Deg2Rad;
            
            _disc.Radius = radius;
            _disc.AngRadiansStart = rad;
            _disc.AngRadiansEnd = 0;
            
            Show();
        }
        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);
    }
}