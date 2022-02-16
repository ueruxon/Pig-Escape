using UnityEngine;
using Shapes;

namespace Game.Code.Logic.Bombs
{
    public class RadiusIndicator : MonoBehaviour
    {
        [SerializeField] private Disc _disc;
        
        public void Init(float radius)
        {
            _disc.Radius = radius;
            Show();
        }

        private void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);
    }
}