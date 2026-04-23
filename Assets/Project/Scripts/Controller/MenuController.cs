using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject settings;

    public void SettingsMenu()
    {
        bool isActive = settings.activeSelf;
        
        settings.SetActive(!isActive);
        Time.timeScale = isActive ? 1 : 0;
    }
   
}
