using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private MenuController menuController;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SettingsManager()
    {
        menuController.SettingsMenu();
    }

    public void YouDieManager()
    {
        menuController.YouDieMenu();
    }
    public bool IsPaused { get; set; }
}
