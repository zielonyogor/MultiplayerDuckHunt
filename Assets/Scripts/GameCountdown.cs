using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;

public class GameCountdown : NetworkBehaviour
{
    public static UnityEvent OnGameCountdownEnd = new UnityEvent();
    private Animator animator;
    private TextMeshProUGUI numberText;

    [SyncVar]
    private int countDownNumber = 3;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        numberText = GetComponent<TextMeshProUGUI>();
    }

    public override void OnStartServer()
    {
        StartCoroutine(PlayCountdown());
    }
    private IEnumerator PlayCountdown()
    {
        while (animator && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        EndCountdown();
    }

    public void ChangeNumber(int number)
    {
        countDownNumber = number;
        numberText.text = countDownNumber.ToString();
    }

    [Server]
    private void EndCountdown()
    {
        OnGameCountdownEnd.Invoke();
        NetworkServer.Destroy(gameObject);
    }
}