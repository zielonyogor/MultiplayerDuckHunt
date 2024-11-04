using TMPro;
using UnityEngine;
using Mirror;

public class GameContainer : NetworkBehaviour {
    public TextMeshProUGUI player1Score, player2Score;
    public Camera mainCamera;

    [Command(requiresAuthority = false)]
    public void CmdUpdateScore(int playerID)
    {
        if(playerID == 0)
        {
            player1Score.text = "00";
        }
        else
        {
            player2Score.text = "00";
        }
    }
}