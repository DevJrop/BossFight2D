using UnityEngine;
using UnityEngine.UI;

namespace Project.Prefabs.Characters.Proyectiles
{
    public class PowerUpUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PowerUp powerUp;
        [SerializeField] private Image fillImage;
        
        private void Start()
        {
            if (powerUp != null)
            {
                powerUp.OnManaChanged += UpdateManaBar;
                
                UpdateManaBar(powerUp.GetManaNormalized());
            }
        }
        private void OnDestroy()
        {
            if (powerUp != null)
            {
                powerUp.OnManaChanged -= UpdateManaBar;
            }
        }
        
        private void UpdateManaBar(float normalizedMana)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = normalizedMana;
            }
        }
    }
}