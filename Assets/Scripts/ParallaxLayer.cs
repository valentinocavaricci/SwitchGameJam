using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float moveSpeed = 0.08f;
    public float resetX = -8f;
    public float loopDistance = 16f;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x <= resetX)
        {
            transform.position += new Vector3(loopDistance, 0f, 0f);
        }
    }
}