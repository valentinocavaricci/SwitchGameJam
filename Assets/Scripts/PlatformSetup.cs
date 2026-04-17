using UnityEngine;

public class PlatformSetup : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool isRed;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

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
}