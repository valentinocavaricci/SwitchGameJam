using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 6f;
    public float deathY = -6f;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isRed = true;
    private bool isGameOver = false;
    private bool isPaused = false;

    private int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        UpdateColor();
        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (isPaused)
            return;

        if (transform.position.y < deathY)
        {
            GameOver();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRed = !isRed;
            UpdateColor();
        }
    }

    void UpdateColor()
    {
        if (isRed)
            sr.color = Color.red;
        else
            sr.color = Color.blue;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver || isPaused)
            return;

        PlatformSetup platform = collision.gameObject.GetComponent<PlatformSetup>();

        if (platform != null)
        {
            if (platform.isRed != isRed)
            {
                GameOver();
                return;
            }

            if (!platform.hasBeenScored)
            {
                platform.hasBeenScored = true;
                score++;
                UpdateScoreUI();
                platform.StartDisappear();
            }
        }
    }

    void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}