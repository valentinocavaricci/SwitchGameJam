using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 6f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isRed = true;
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Time.timeScale = 1f;
        UpdateColor();
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
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
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.blue;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlatformSetup platform = collision.gameObject.GetComponent<PlatformSetup>();

        if (platform != null)
        {
            if (platform.isRed != isRed)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over - Press R to Restart");
        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}