using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float timer;
    private void OnEnable()
    {
        lifeTime = lifeTime;
    }
    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
