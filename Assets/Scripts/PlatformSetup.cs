using UnityEngine;

public class PlatformSetup : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    public bool isRed;
    public bool hasBeenScored = false;
    public bool isDisappearing = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        float randomWidth = Random.Range(2f, 4.5f);
        transform.localScale = new Vector3(randomWidth, transform.localScale.y, 1f);

        isRed = Random.value < 0.5f;

        if (isRed)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.blue;
        }
    }

    public void StartDisappear()
    {
        if (!isDisappearing)
        {
            isDisappearing = true;
            Invoke(nameof(Disappear), 0.15f);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}