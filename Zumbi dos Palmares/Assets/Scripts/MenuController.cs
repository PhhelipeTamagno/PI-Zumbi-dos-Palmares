using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
   
    public GameObject pauseMenu;  
    public GameObject imageMenu;  
    public Button backButton;    
    public Button openImageMenuButton;  
    public Button closeImageMenuButton; 

    private bool isPaused = false;  

    void Start()
    {
       
        pauseMenu.SetActive(false);
        imageMenu.SetActive(false);

       
        backButton.onClick.AddListener(BackToMainMenu);
        openImageMenuButton.onClick.AddListener(OpenImageMenu);
        closeImageMenuButton.onClick.AddListener(CloseImageMenu);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                CloseAllMenus();  
            }
            else
            {
                OpenPauseMenu();  
            }
        }
    }

    
    public void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);   
        imageMenu.SetActive(false);  
        Time.timeScale = 0f;         
    }

    
    public void CloseAllMenus()
    {
        
        pauseMenu.SetActive(false);
        imageMenu.SetActive(false);
        Time.timeScale = 1f;  
        isPaused = false;     
    }

    
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("Menu 0");  
    }

    
    public void OpenImageMenu()
    {
        pauseMenu.SetActive(false);   
        imageMenu.SetActive(true);    
    }

    
    public void CloseImageMenu()
    {
        imageMenu.SetActive(false);   
        pauseMenu.SetActive(true);    
    }
}
