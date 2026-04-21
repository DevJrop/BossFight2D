using UnityEngine;

namespace Project.Scripts.Controller
{
    public class ExplosionAutoDestroy : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 1f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}