using UnityEngine;

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
   
}
