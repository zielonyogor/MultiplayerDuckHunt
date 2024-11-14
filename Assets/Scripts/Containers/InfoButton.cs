using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    private Button infoButton;
    private GameObject info;

    private void Start()
    {
        info = transform.GetChild(1).gameObject;
        infoButton = GetComponent<Button>();

        infoButton.onClick.AddListener(ShowInfo);
    }

    public void ShowInfo()
    {
        infoButton.onClick.RemoveAllListeners();
        infoButton.onClick.AddListener(HideInfo);
        info.SetActive(true);
    }

    public void HideInfo()
    {
        infoButton.onClick.RemoveAllListeners();
        infoButton.onClick.AddListener(ShowInfo);
        info.SetActive(false);
    }
}
