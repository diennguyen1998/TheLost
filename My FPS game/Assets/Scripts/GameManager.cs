using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float restartDelay = 1f;
    public GameObject deadUI;
    public GameObject enemy;
    private EnemyController enemyController;
    private PlayerMove playerMove;
    private PlayerView playerView;
    private GameObject player;

    void Start()
    {
        enemyController = FindObjectOfType<EnemyController>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerView = FindObjectOfType<PlayerView>();
        player = GameObject.FindWithTag("Player");
        //EnemyDrop();
    }

    public void EndGame()
    {
        playerMove.enabled = false;
        enemyController.enabled = false;
        StartCoroutine(DeadScene());
    }

    IEnumerator DeadScene()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2f);
        deadUI.SetActive(true);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void EnemyDrop()
    {
        int xPos = Random.Range(11, 50);
        int zPos = Random.Range(11, 31);
        enemy.transform.position = new Vector3(xPos, 0, zPos);
    }
}
