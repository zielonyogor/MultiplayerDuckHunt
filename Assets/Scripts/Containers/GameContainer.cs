using TMPro;
using UnityEngine;
using Mirror;

public class GameContainer : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI player1ScoreText, player2ScoreText;

    [SyncVar(hook = nameof(OnPlayer1ScoreChanged))]
    private int player1Score;

    [SyncVar(hook = nameof(OnPlayer2ScoreChanged))]
    private int player2Score;

    public Camera mainCamera;

    [Command(requiresAuthority = false)]
    public void CmdUpdateScore(int playerID)
    {
        if (playerID == 0)
        {
            player1Score += 10;
        }
        else
        {
            player2Score += 10;
        }
    }

    private void OnPlayer1ScoreChanged(int oldScore, int newScore)
    {
        player1ScoreText.text = newScore.ToString("D6");
    }
    private void OnPlayer2ScoreChanged(int oldScore, int newScore)
    {
        player2ScoreText.text = newScore.ToString("D6");
    }
}