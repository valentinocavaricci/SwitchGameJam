using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    public float destroyX = -6f;

    void Update()
    {
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
