using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;      
      private bool isPaused = false;     

    void Start()
    {
        pausePanel.SetActive(false);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
        }
    }

               
    public void BackToPauseMenu()
    {
     pausePanel.SetActive(true);     
    }

    
    public void CloseAllPanels()
    {
        pausePanel.SetActive(false);
       
        isPaused = false;
        Time.timeScale = 1; 
    }
}
