using UnityEngine;

public class InputManager : MonoBehaviour
{
    MenuController menuController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.SettingsManager();
        }
    }
}
