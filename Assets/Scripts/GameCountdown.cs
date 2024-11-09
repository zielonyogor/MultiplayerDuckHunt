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

    public override void OnStartServer()
    {
        animator = GetComponent<Animator>();
        numberText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(PlayCountdown());
    }
    private IEnumerator PlayCountdown()
    {
        while (animator && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
        OnGameCountdownEnd.Invoke();
        Destroy(gameObject);
    }

    public void ChangeNumber(int number)
    {
        numberText.text = number.ToString();
    }
}