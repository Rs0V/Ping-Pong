using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public float TimeMultiplier = 0.5f;
    public float restartTime = 2f;
    float resTimer;
    bool restart;

    public float timeBeforeSpeedBoost;
    public float speedBoost;
    float sbTimer;
    public Animator speedUpAnimator;

    bool beginGame;

    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Random.Range(0, 100000));
        float chance = Random.Range(0f, 100f);
        if(chance < 50f)
        {
            player1.GetComponent<BallBehavior>().serve = true;
            player1.GetComponent<BallBehavior>().Initialize();
            player2.GetComponent<BallBehavior>().serve = false;
        }
        else
        {
            player1.GetComponent<BallBehavior>().serve = false;
            player2.GetComponent<BallBehavior>().serve = true;
            player2.GetComponent<BallBehavior>().Initialize();
        }
        Time.timeScale = TimeMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if(beginGame == true)
        {
            if (restart == true)
            {
                resTimer += Time.deltaTime / TimeMultiplier;
                if (resTimer > restartTime)
                {
                    restart = false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else
            {
                sbTimer += Time.deltaTime / TimeMultiplier;
                if (sbTimer > timeBeforeSpeedBoost)
                {
                    sbTimer = 0f;
                    TimeMultiplier += speedBoost;
                    Time.timeScale = TimeMultiplier;
                    speedUpAnimator.speed = 1 / TimeMultiplier;
                    speedUpAnimator.SetTrigger("PlayAnim");
                }
            }
        }
    }

    public void RestartScene(GameObject ball, Vector3 velocity)
    {
        ball.GetComponent<BallMovement>().inGameColl.enabled = false;
        ball.GetComponent<BallMovement>().loseGameColl.enabled = true;
        ball.GetComponent<BallMovement>().loseGameRB.isKinematic = false;
        resTimer = 0f;
        restart = true;
        ball.GetComponent<BallMovement>().loseGameRB.velocity = velocity;
    }

    public void FirstHit()
    {
        beginGame = true;
    }
}
