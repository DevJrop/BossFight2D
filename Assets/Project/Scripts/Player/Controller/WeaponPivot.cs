using UnityEngine;

namespace Project.Scripts.Player.Controller
{
    public class WeaponPivot : MonoBehaviour
    {
        [SerializeField] private Transform gunPosition;
        void Update()
        {
            transform.position = gunPosition.position;
        }
    }
}
