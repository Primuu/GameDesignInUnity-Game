using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    public AudioClip enterPipeSound;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player")) {
            if (Input.GetKey(enterKeyCode)) {
                StartCoroutine(Enter(other.transform));
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f;

        AudioManager.Instance.PlaySFX(enterPipeSound);

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f);

        bool underground = connection.position.y < 0f;
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underground);

        // If the player is to be moved after exiting the pipe
        if (exitDirection != Vector3.zero) {
            // Position from which the player starts the exit animation from the pipe
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        } else {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float animationPercentage = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, animationPercentage);
            player.localScale = Vector3.Lerp(startScale, endScale, animationPercentage);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }

}
