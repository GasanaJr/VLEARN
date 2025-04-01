using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class ShowOnHover : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public string objectInfo = "This is an important object";

    void Start()
    {
       if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        } 
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        ShowInfo();
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        HideInfo();
    }

    void ShowInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            infoText.text = objectInfo;
        }
    }

    void HideInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

}
