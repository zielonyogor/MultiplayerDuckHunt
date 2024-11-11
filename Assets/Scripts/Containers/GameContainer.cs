using TMPro;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameContainer : NetworkBehaviour
{
    [Header("Score text")]
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    [Header("Timer")]
    public GameTimer timer;
    [Header("Game over screen")]
    [SerializeField] GameObject gameOverScreen;

    [SyncVar(hook = nameof(OnPlayer1ScoreChanged))]
    private int player1Score = 0;

    [SyncVar(hook = nameof(OnPlayer2ScoreChanged))]
    private int player2Score = 0;

    public override void OnStartClient()
    {
        GameTimer.OnTimerEnd.AddListener(HandleGameOver);
    }

    private void HandleGameOver()
    {
        Debug.Log("Time is up!");
        gameOverScreen.SetActive(true);
        TextMeshProUGUI resultText = gameOverScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Debug.Log(player1Score + " i " + player2Score);
        if (player1Score > player2Score)
        {
            resultText.text = "Player 1 wins!!";
        }
        else
        {
            resultText.text = "Player 2 wins!!";
        }

        if (isServer)
        {
            Button mainMenuButton = gameOverScreen.transform.GetChild(2).GetComponent<Button>();
            mainMenuButton.onClick.AddListener(() =>
            {
                Debug.Log("here we go to main menu - add reference to CustomNetworkManager")
            });
        }
    }

    public void UpdateScore(int playerID, int score)
    {
        if (playerID == 0)
        {
            player1Score += score;
        }
        else
        {
            player2Score += score;
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