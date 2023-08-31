using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI hintText;

    [SerializeField] private string[] hintMessages;
    [SerializeField] private Image leftArrowImage;
    [SerializeField] private Image rightArrowImage;

    private int arrowDisabledOpacity = 60;
    private bool hintPanelVisibility;
    private int currHintIndex;
    void Start()
    {
        HideAllPanels();

        InitializeHintPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(Controls.ToggleHint)){
            ToggleHintPanelVisibility();
        }

        if(Input.GetKeyDown(Controls.NextHint)){ // ->
            DisplayNextHintMessage();
        }
        else if(Input.GetKeyDown(Controls.PrevHint)){  // <- 
            DisplayPreviousHintMessage();
        }
    }

    private void InitializeHintPanel(){
        hintPanelVisibility = false;
        currHintIndex = 0;

        SetHintMessage(hintMessages[currHintIndex]);

        ToggleArrowOpacity(leftArrowImage, arrowDisabledOpacity);
    }

    private void HideAllPanels(){
        foreach(Transform child in this.transform){
            child.gameObject.SetActive(false);
        }

        hintPanelVisibility = false;
    }

    private void ToggleArrowOpacity(Image arrow, int opacity){
        Color tempColor = arrow.color;
        
        arrow.color = new Color(tempColor.r, tempColor.g, tempColor.b, opacity / 255f);
    }

    private void DisplayNextHintMessage(){
        if(!hintPanelVisibility || currHintIndex >= hintMessages.Length - 1){
            return;
        }

        ToggleArrowOpacity(leftArrowImage, 255);

        SetHintMessage(hintMessages[++currHintIndex]);

        if(currHintIndex >= hintMessages.Length - 1){
            ToggleArrowOpacity(rightArrowImage, arrowDisabledOpacity);
        }
    }

    private void DisplayPreviousHintMessage(){
        if(!hintPanelVisibility || currHintIndex <= 0){
            return;
        }

        ToggleArrowOpacity(rightArrowImage, 255);

        SetHintMessage(hintMessages[--currHintIndex]);

        if(currHintIndex <= 0){
            ToggleArrowOpacity(leftArrowImage, arrowDisabledOpacity);
        }
    }

    private void ToggleHintPanelVisibility(){
        hintPanelVisibility = !hintPanelVisibility;
        hintText.transform.parent.gameObject.SetActive(hintPanelVisibility);
    }

    public void DisplayPickUpMessage(string objectName){
        messageText.SetText("Press F to Pick up " + objectName);
        messageText.transform.parent.gameObject.SetActive(true);
    }

    public void HidePickUpMessage(){
        messageText.transform.parent.gameObject.SetActive(false);
    }

    private void SetHintMessage(string hint){
        hintText.SetText(hint);
    }
}
