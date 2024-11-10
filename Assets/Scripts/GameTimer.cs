using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : NetworkBehaviour
{
    [SerializeField] float roundTime = 120f;
    private TextMeshProUGUI timerText;

    public static UnityEvent OnTimerEnd = new UnityEvent();

    [SyncVar(hook = nameof(OnTimeChange))]
    public float currentTime;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public override void OnStartServer()
    {
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
            yield return null;
        }
        OnTimerEnd.Invoke();
    }

    private void OnTimeChange(float oldTime, float newTime)
    {
        currentTime = newTime;
        timerText.text = TimeSpan.FromSeconds(currentTime).ToString("mm':'ss'.'f");
    }
}
