using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Controller
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
