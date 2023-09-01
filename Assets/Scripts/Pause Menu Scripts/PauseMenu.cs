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

    [SerializeField] TextMeshProUGUI[] controlInputs;
    void Start()
    {
        togglePauseMenu = false;
        pauseMenuCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        isInControlsMenu = false;

        SetControlKeys();
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

    private void SetControlKeys(){
        for(int i = 0; i < controlInputs.Length; i++){
            switch(controlInputs[i].gameObject.name){
                case "Go Forward":
                    controlInputs[i].SetText(Controls.GoForward.ToString());
                    break;

                case "Go Backward":
                    controlInputs[i].SetText(Controls.GoBackward.ToString());
                    break;

                case "Rotate Left":
                    controlInputs[i].SetText(Controls.RotateLeft.ToString());
                    break;

                case "Rotate Right":
                    controlInputs[i].SetText(Controls.RotateRight.ToString());
                    break;

                case "Run":
                    controlInputs[i].SetText(Controls.Running.ToString());
                    break;

                case "Pick Up Object":
                    controlInputs[i].SetText(Controls.PickUpObj.ToString());
                    break;

                case "Use Object":
                    controlInputs[i].SetText(Controls.UseObj.ToString());
                    break;

                case "Drop Object":
                    controlInputs[i].SetText(Controls.DropObj.ToString());
                    break;

                case "Reset Player":
                    controlInputs[i].SetText(Controls.ResetPlayer.ToString());
                    break;

                case "Toggle Hints":
                    controlInputs[i].SetText(Controls.ToggleHint.ToString());
                    break;
                
                case "Next Hint":
                    controlInputs[i].SetText(Controls.NextHint.ToString());
                    break;

                case "Previous Hint":
                    controlInputs[i].SetText(Controls.PrevHint.ToString());
                    break;
            }
        }
    }
}
