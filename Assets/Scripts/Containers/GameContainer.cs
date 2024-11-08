using TMPro;
using UnityEngine;
using Mirror;

public class GameContainer : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI player1ScoreText, player2ScoreText;

    [SyncVar(hook = nameof(OnPlayer1ScoreChanged))]
    private int player1Score = 0;

    [SyncVar(hook = nameof(OnPlayer2ScoreChanged))]
    private int player2Score = 0;
    public Camera mainCamera;

    public override void OnStartServer()
    {
        PigeonManager.OnPigeonShot.AddListener(UpdateScore);
        Debug.Log("skibidi");
    }

    public void UpdateScore(int playerID, int pigeonID)
    {
        Debug.Log("Updating score");
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