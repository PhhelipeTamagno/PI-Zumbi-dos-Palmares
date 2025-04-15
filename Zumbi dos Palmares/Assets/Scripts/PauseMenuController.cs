using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;      
    public GameObject settingsPanel;   
    private bool isPaused = false;     

    void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausePanel();
        }
    }

    
    public void TogglePausePanel()
    {
        isPaused = !isPaused;

        
        pausePanel.SetActive(isPaused);
        settingsPanel.SetActive(false);

        
        Time.timeScale = isPaused ? 0 : 1;
    }

    
    public void OpenSettings()
    {
        pausePanel.SetActive(false);   
        settingsPanel.SetActive(true); 
    }

    
    public void BackToPauseMenu()
    {
        settingsPanel.SetActive(false); 
        pausePanel.SetActive(true);     
    }

    
    public void CloseAllPanels()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1; 
    }
}
