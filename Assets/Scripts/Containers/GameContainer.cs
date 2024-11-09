using TMPro;
using UnityEngine;
using Mirror;

public class GameContainer : NetworkBehaviour
{
    [Header("Score text")]
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    [Header("Timer")]
    public GameTimer timer;


    [SyncVar(hook = nameof(OnPlayer1ScoreChanged))]
    private int player1Score = 0;

    [SyncVar(hook = nameof(OnPlayer2ScoreChanged))]
    private int player2Score = 0;

    public override void OnStartServer()
    {
        PigeonManager.OnPigeonShot.AddListener(UpdateScore);
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