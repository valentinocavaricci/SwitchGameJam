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

    public AudioClip jumpSound;
    public AudioClip switchSound;
    public AudioClip scoreSound;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    
    public AudioSource backgroundMusicSource;

    private bool isRed = true;
    private bool isGameOver = false;
    private bool isPaused = false;

    private int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

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
            PlaySound(jumpSound);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRed = !isRed;
            UpdateColor();
            PlaySound(switchSound);
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
                PlaySound(scoreSound);
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

        PlaySound(gameOverSound);

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }

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
        PlayButtonClick();

        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        PlayButtonClick();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayButtonClick()
    {
        PlaySound(buttonClickSound);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}