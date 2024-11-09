using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : NetworkBehaviour
{
    [SerializeField] float roundTime = 100f;
    private TextMeshProUGUI timerText;

    public static UnityEvent OnTimerEnd = new UnityEvent();

    [SyncVar]
    public float currentTime;

    public override void OnStartServer()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        currentTime = roundTime;
        timerText.text = TimeSpan.FromSeconds(currentTime).ToString("mm':'ss'.'f");
        GameCountdown.OnGameCountdownEnd.AddListener(() =>
        {
            StartCoroutine(UpdateTimer());
        });
    }

    private IEnumerator UpdateTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = TimeSpan.FromSeconds(currentTime).ToString("mm':'ss'.'f");
            yield return null;
        }
        OnTimerEnd.Invoke();
    }
}
