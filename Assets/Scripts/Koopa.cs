using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    private bool shelled;
    private bool shellMoving;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();

            if (collision.transform.DotTest(transform, Vector2.down)) {
                EnterShell();
            } else {
                player.Hit();
            }
        }
    }

    private void EnterShell()
    {
        shelled = true;
        
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

}
