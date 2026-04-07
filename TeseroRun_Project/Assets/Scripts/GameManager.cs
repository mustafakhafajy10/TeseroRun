using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private static int score;
    public TMP_Text keyText;
    private static int keyScore = 10;
    private Rigidbody2D rb;
    public GameObject NPCHelper;
    public GameObject NPCPrefab;
    public static int LevelScore = 0;
    public static GameManager Instance;

    private GameObject currentNPC; // Tracks the currently spawned NPC

    private string[] sceneNames = { "MainMenu", "Level1" };

    // Start is called before the first frame update
    //private bool isNPCActive = false; // Tracks if an NPC is active

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Coins: " + score;
        }

        if (keyText != null)
        {
            keyText.text = "Remaining Keys: " + keyScore;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!gameObject.activeSelf)
        {
            Debug.LogWarning("GameManager is inactive. Update logic will not execute.");
            return;
        }

        if (NPCHelper == null)
        {
            return;
        }

        if (score >= 80)
        {
            NPCHelper.gameObject.SetActive(true);
        }
        else
        {
            NPCHelper.gameObject.SetActive(false);
        }
    }

    public int GetKeyScore() // New method to get the current score
    {
        return keyScore;
    }

    public void setKeyScore(int newKeyScore) // New method to get the current score
    {
        keyScore = newKeyScore;
        UpdateText();
    }

    public int GetScore() // New method to get the current score
    {
        return score;
    }

    public void setScore(int newScore) // New method to get the current score
    {
        score = newScore;
        UpdateText();
    }

    public void AddScore(int points)
    {
        score += points; // Increment coins
        UpdateText();
    }

    //void UpdateText()
    //{
    //    scoreText.text = "Coins: " + score;
    //    keyText.text = "Remaining Keys: " + keyScore;
    //}

    //public void RemovePoints(int points)
    //{
    //    return score; // Return the current score as coins
    //}

    public void AddKey(int points)
    {
        keyScore -= points; // Decrement keys
        UpdateText();

        Debug.Log($"Current Keys: {Mathf.Max(0, keyScore)}");
    }

    void UpdateText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Coins: " + score;
        }

        if (keyText != null)
        {
            keyText.text = "Remaining Keys: " + Mathf.Max(0, keyScore);
        }
    }

    public void removePoints(int points)
    {
        score -= points; // Decrement coins
        score = Mathf.Max(0, score); // Ensure score doesn't go negative
        UpdateText();
    }

    public int GetCoins()
    {
        return score;
    }

    void LoadNextLevel()
    {
        int currentLevelIndex = System.Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name); // Get current scene index
        int nextLevelIndex = currentLevelIndex + 1; // Increment to load the next scene

        // Check if the next level exists in the array
        if (nextLevelIndex < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[nextLevelIndex]);
        }
        else
        {
            Debug.Log("Game Over or All Levels Complete!");
            SceneManager.LoadScene(sceneNames[0]);
        }
    }
        //if (keyScore <= 0)
        //{
        //    LoadNextScene();
        //    keyScore = 10;
        //}
    }

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
