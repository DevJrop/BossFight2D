using UnityEngine;

namespace Project.Scripts.Controller
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject settings;

        public void SettingsMenu()
        { 
            bool isActive = settings.activeSelf;
        
            settings.SetActive(!isActive);
        
            UIManager.instance.IsPaused = !isActive;
            Time.timeScale = isActive ? 1 : 0;
        }

        public void YouDieMenu(GameObject youDie)
        {
            bool isActive = youDie.activeSelf;
            youDie.SetActive(!isActive);
            UIManager.instance.IsPaused = !isActive;
            Time.timeScale = isActive ? 1 : 0;
        }
    
   
    }
}
