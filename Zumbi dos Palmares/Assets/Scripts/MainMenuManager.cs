using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject normalMenu;
    public GameObject completedMenu;

    void Start()
    {
        bool gameCompleted = PlayerPrefs.GetInt("GameCompleted", 0) == 1;

        if (gameCompleted)
        {
            normalMenu.SetActive(false);
            completedMenu.SetActive(true);
            Debug.Log("Menu alternado para pós-jogo!");
        }
        else
        {
            normalMenu.SetActive(true);
            completedMenu.SetActive(false);
        }
    }
}
