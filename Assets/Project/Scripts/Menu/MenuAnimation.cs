using System.Collections;
using UnityEngine;
using DG.Tweening;
namespace Project.Scripts.Menu
{
    public class MenuAnimation : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [SerializeField] private GameObject[] spawner;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float timeBullets = 1f;

        [Header("Menu State")]
        [SerializeField] private bool isMenu = true;

        private void Start()
        {
            if (isMenu)
            {
                StartCoroutine(MovementBullets());
            }
        }
        private IEnumerator MovementBullets()
        {
            while (isMenu)
            {
                foreach (var bullet in spawner)
                {
                    GameObject newBullet = Instantiate(
                        bullet,
                        spawnPoint.position,
                        spawnPoint.rotation
                    );
                    newBullet.transform.DOMove(
                        new Vector3(endPoint.position.x, endPoint.position.y, endPoint.position.z),
                        2f
                    );
                    yield return new WaitForSeconds(timeBullets);
                }
            }
        }
    }
}