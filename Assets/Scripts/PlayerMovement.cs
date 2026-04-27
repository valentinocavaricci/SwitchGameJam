using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 6f;
    public float deathY = -6f;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject modeText;
    public PlatformSpawner platformSpawner;

    public AudioClip jumpSound;
    public AudioClip switchSound;
    public AudioClip scoreSound;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;

    public AudioSource backgroundMusicSource;

    public float stretchAmount = 1.2f;
    public float squashAmount = 0.85f;
    public float squashStretchTime = 0.06f;

    public float oppositeModeInterval = 10f;
    public float oppositeModeDuration = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private bool isRed = true;
    private bool isGameOver = false;
    private bool isPaused = false;
    private bool isOppositeMode = false;

    private int score = 0;

    private Vector3 originalScale;
    private Coroutine squashStretchRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        originalScale = transform.localScale;

        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (modeText != null)
            modeText.SetActive(false);

        UpdateColor();
        UpdateScoreUI();

        StartCoroutine(OppositeModeRoutine());
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
            PlayJumpStretch();
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
            bool isCorrectHit;

            if (isOppositeMode)
                isCorrectHit = (platform.isRed != isRed);
            else
                isCorrectHit = (platform.isRed == isRed);

            if (!isCorrectHit)
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

                if (platformSpawner != null)
                {
                    platformSpawner.UpdateDifficulty(score);
                }

                platform.StartDisappear();
            }
        }
    }

    IEnumerator OppositeModeRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(oppositeModeInterval);

            if (isGameOver)
                yield break;

            isOppositeMode = true;

            if (modeText != null)
                modeText.SetActive(true);

            yield return new WaitForSeconds(oppositeModeDuration);

            isOppositeMode = false;

            if (modeText != null)
                modeText.SetActive(false);
        }
    }

    void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (modeText != null)
            modeText.SetActive(false);

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

    void PlayJumpStretch()
    {
        if (squashStretchRoutine != null)
        {
            StopCoroutine(squashStretchRoutine);
        }

        squashStretchRoutine = StartCoroutine(JumpStretchRoutine());
    }

    IEnumerator JumpStretchRoutine()
    {
        transform.localScale = new Vector3(originalScale.x * squashAmount, originalScale.y * stretchAmount, originalScale.z);
        yield return new WaitForSeconds(squashStretchTime);

        transform.localScale = new Vector3(originalScale.x * stretchAmount, originalScale.y * squashAmount, originalScale.z);
        yield return new WaitForSeconds(squashStretchTime);

        transform.localScale = originalScale;
    }
}