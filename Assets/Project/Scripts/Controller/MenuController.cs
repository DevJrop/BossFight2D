using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject youDie;

    public void SettingsMenu()
    { 
        bool isActive = settings.activeSelf;
        
        settings.SetActive(!isActive);
        
        UIManager.instance.IsPaused = !isActive;
        Time.timeScale = isActive ? 1 : 0;
    }

    public void YouDieMenu()
    {
        bool isActive = youDie.activeSelf;
        youDie.SetActive(!isActive);
        UIManager.instance.IsPaused = !isActive;
        Time.timeScale = isActive ? 1 : 0;
    }
    
   
}
