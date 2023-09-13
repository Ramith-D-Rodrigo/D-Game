using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI[] controlInputs;
    void Start()
    {
        SetControlKeys(controlInputs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetControlKeys(TextMeshProUGUI[] controlsInputTextGUIs){
        for(int i = 0; i < controlsInputTextGUIs.Length; i++){
            switch(controlsInputTextGUIs[i].gameObject.name){
                case "Go Forward":
                    controlsInputTextGUIs[i].SetText(Controls.GoForward.ToString());
                    break;

                case "Go Backward":
                    controlsInputTextGUIs[i].SetText(Controls.GoBackward.ToString());
                    break;

                case "Rotate Left":
                    controlsInputTextGUIs[i].SetText(Controls.RotateLeft.ToString());
                    break;

                case "Rotate Right":
                    controlsInputTextGUIs[i].SetText(Controls.RotateRight.ToString());
                    break;

                case "Run":
                    controlsInputTextGUIs[i].SetText(Controls.Running.ToString());
                    break;

                case "Pick Up Object":
                    controlsInputTextGUIs[i].SetText(Controls.PickUpObj.ToString());
                    break;

                case "Use Object":
                    controlsInputTextGUIs[i].SetText(Controls.UseObj.ToString());
                    break;

                case "Drop Object":
                    controlsInputTextGUIs[i].SetText(Controls.DropObj.ToString());
                    break;

                case "Reset Player":
                    controlsInputTextGUIs[i].SetText(Controls.ResetPlayer.ToString());
                    break;

                case "Toggle Hints":
                    controlsInputTextGUIs[i].SetText(Controls.ToggleHint.ToString());
                    break;
                
                case "Next Hint":
                    controlsInputTextGUIs[i].SetText(Controls.NextHint.ToString());
                    break;

                case "Previous Hint":
                    controlsInputTextGUIs[i].SetText(Controls.PrevHint.ToString());
                    break;
                
                case "Inventory Item 1":
                    controlsInputTextGUIs[i].SetText("1");
                    break;

                case "Inventory Item 2":
                    controlsInputTextGUIs[i].SetText("2");
                    break;

                case "Inventory Item 3":
                    controlsInputTextGUIs[i].SetText("3");
                    break;
            }
        }
    }
}
