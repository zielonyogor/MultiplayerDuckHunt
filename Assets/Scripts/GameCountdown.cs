using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCountdown : MonoBehaviour
{
    [SerializeField] float roundTime = 100f;
    private TextMeshProUGUI timerText;
    private float currentTime;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        currentTime = roundTime;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = TimeSpan.FromSeconds(currentTime).ToString("mm':'ss'.'f");
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
