﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float restartDelay = 1f;
    public GameObject deadUI;
    private EnemyController enemyController;
    private PlayerMove playerMove;
    private PlayerView playerView;

    void Start()
    {
        enemyController = FindObjectOfType<EnemyController>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerView = FindObjectOfType<PlayerView>();
    }

    public void EndGame()
    {
        playerMove.enabled = false;
        enemyController.enabled = false;
        deadUI.SetActive(true);
        Invoke("Restart", restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
