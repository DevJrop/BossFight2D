using UnityEngine;

namespace Project.Scripts.Controller
{
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

        public void YouDieManager(GameObject menuWhenDie)
        {
            menuController.YouDieMenu(menuWhenDie);
        }
        public bool IsPaused { get; set; }
    }
}
