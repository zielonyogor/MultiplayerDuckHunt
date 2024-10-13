using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour
{
    public float coolDownTime = 0.3f;

    private float lastShootTime;
    private LayerMask layerMask;
    private Camera mainCamera;

    [SerializeField] private TextMeshProUGUI scoreText;
    public int PlayerScore { get; private set; }

    void Start()
    {
        mainCamera = Camera.main;
        layerMask = LayerMask.GetMask("Birds");
        Debug.Log(layerMask.value);
        PlayerScore = 0;
    }

    void OnShoot()
    {
        if(Time.time - lastShootTime >= coolDownTime)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), Mathf.Infinity, layerMask);

            if(hit.collider != null)
            {
                lastShootTime = Time.time;
                PlayerScore += 10;
                scoreText.text = "Score: " + PlayerScore.ToString("D6");
            }
        }
    }
}
