using UnityEngine;

public class BubbleEffect : MonoBehaviour
{
    public float force = 1f; 
    public bool pushUp = true; 

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                float direction = pushUp ? 1f : -1f;
                rb.AddForce(Vector2.up * direction * force, ForceMode2D.Impulse);
            }
        }
    }
}
