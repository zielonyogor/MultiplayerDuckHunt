using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }
    public TextMeshProUGUI player1Score, player2Score;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

}
