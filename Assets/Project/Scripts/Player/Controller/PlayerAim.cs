using UnityEngine;

namespace Project.Scripts.Player.Controller
{
    public class PlayerAim : MonoBehaviour
    {
        void Update()
        {
            Aim();
        }
        private void Aim()
        {
            Vector3 aimVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimVector.z = 0;
        
            Vector3 direction = aimVector - transform.position;
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
        }
    }
}
