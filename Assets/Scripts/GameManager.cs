using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("The player script")]private Rigidbody player;
    [SerializeField, Tooltip("the current score")]private float currentScore = 0;
    [SerializeField, Tooltip("the text box for the player's score")]private TMP_Text scoreText;
    [SerializeField, Tooltip("the game over menu")]private GameObject GameOverMenu;
    //[SerializeField, Tooltip("the game over menu")]private GameObject GameOverMenu2;
    [SerializeField, Tooltip("the game over score text field")]private TMP_Text gameOverScoreText;
    private bool gameOver = false;
    [SerializeField, Tooltip("the rate at which the score increase per second")]private float pointsPerSecond;
    private static GameManager _instance;

    public static GameManager Instance
    {
        get{
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<GameManager>();
                    Debug.Log("Generating new game manager");
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameOverMenu.SetActive(false);
        // if(GameOverMenu2){
        //     GameOverMenu2.SetActive(false);
        // }
        
        currentScore = 0;
        
        UpdateScoreText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float pointsRate = pointsPerSecond;
        if(player){
            pointsRate += Mathf.Max(player.velocity.x, 0);
        }
        IncreaseScore(pointsRate * Time.fixedDeltaTime);
    }

    

    /// <summary>
    /// returns the player's current score
    /// </summary>
    /// <returns></returns>
    public float GetScore(){
        return currentScore;
    }

    /// <summary>
    /// sets the player's score to the given value (does nothing if less than zero)
    /// </summary>
    /// <param name="score">the score the player will have</param>
    public void UpdateScore(float score){
        if(score < 0){
            return;
        }
        currentScore = score;
        UpdateScoreText();
    }

    /// <summary>
    /// adds the given amount to the player's score
    /// </summary>
    /// <param name="score">the amount to increase the score by</param>
    public void IncreaseScore(float score){
        //Debug.Log("added points");
            currentScore += score;
       

        UpdateScoreText();
    }

    private void UpdateScoreText(){
        scoreText.text = "Score: " + ((int)currentScore).ToString();
    }

    public void GameOver(){
        gameOver = true;
        gameOverScoreText.text = "Final Score: " + ((int)currentScore).ToString();
        GameOverMenu.SetActive(true);
        // if(GameOverMenu2){
        //     GameOverMenu2.SetActive(true);
        // }
    }

    public bool isPlaying(){
        return !gameOver;
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu(){
        SceneManager.LoadScene(0);
    }
    public void Quit(){
        Application.Quit();
    }
}
