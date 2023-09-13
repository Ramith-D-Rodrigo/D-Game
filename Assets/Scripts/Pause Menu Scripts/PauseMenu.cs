using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private bool togglePauseMenu;
    private bool isInControlsMenu;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject controlsCanvas;
    [SerializeField] ControlsMenu controlsMenu;
    void Start()
    {
        togglePauseMenu = false;
        pauseMenuCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        isInControlsMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(Controls.ToggleMenu)){
            TogglePauseMenuVisibility();
        }
    }

    private void TogglePauseMenuVisibility(){
        if(isInControlsMenu){   //cannot exit from controls menu
            return;
        }
        togglePauseMenu = !togglePauseMenu;
        pauseMenuCanvas.SetActive(togglePauseMenu);

        if(togglePauseMenu){    //pause menu active
            Time.timeScale = 0.0f;
        }
        else{   //pause menu deactivate
            Time.timeScale = 1.0f;
        }
    }

    public void ClickResumeButton(){
        togglePauseMenu = false;
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ClickQuitButton(){
        Debug.Log("Quit the game");
        Application.Quit();
    }

    public void ClickControlsButton(){
        isInControlsMenu = true;
    }

    public void ClickGoBackButton(){
        isInControlsMenu = false;
    }
}
