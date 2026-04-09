using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.Enemy.Movement
{
    public class EnemyMove : MonoBehaviour
    {
       [SerializeField] Transform[] spots;
       int currentSpot;
       private Transform enemy;
       float time;
       private bool isWaiting;
       private int randPos;

       [SerializeField]private float timeSpot;
       [SerializeField]private float pointClose;
       [SerializeField]private float speedBetweenPoints;
       private void Start()
       {
           enemy = GetComponent<Transform>();
       }
       private void Update()
       {
           enemy.position = Vector2.MoveTowards(enemy.position, spots[currentSpot].position, Time.deltaTime * speedBetweenPoints);

           CheckPathing();
       }
       private void CheckPathing()
       {
           Pathing();
       }
       private void Pathing()
       {
           if (Vector2.Distance(enemy.position, spots[currentSpot].position) < pointClose && !isWaiting)
           {
               isWaiting = true;
               StartCoroutine(Wait());
           }
           
       }
       IEnumerator Wait()
       {
           yield return new WaitForSeconds(timeSpot);
           do
           { 
               randPos = Random.Range(0, spots.Length);
           } while (randPos == currentSpot);
           currentSpot = randPos;
           isWaiting = false;
           Debug.Log(currentSpot);
       }
    }
}
