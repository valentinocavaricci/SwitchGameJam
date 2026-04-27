using UnityEngine;

public class PlatformSetup : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool isRed;
    public bool hasBeenScored = false;
    public bool isDisappearing = false;

    public float minWidth = 1f;
    public float maxWidth = 3.7f;
    public float platformHeight = 0.4f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        float randomWidth = Random.Range(minWidth, maxWidth);
        transform.localScale = new Vector3(randomWidth, platformHeight, 1f);

        isRed = Random.value < 0.5f;

        if (isRed)
            sr.color = Color.red;
        else
            sr.color = Color.blue;
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