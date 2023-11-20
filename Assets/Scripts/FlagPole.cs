using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;

    public AudioClip flagSound;
    public AudioClip stageEndSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(flagSound);

            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        AudioManager.Instance.PlaySFX(stageEndSound);

        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(4.5f);

        bool resetPlayer = true;
        GameManager.Instance.LoadLevel(nextWorld, nextStage, resetPlayer);
    }

    private IEnumerator MoveTo(Transform subject, Vector3 destinantion)
    {
        while (Vector3.Distance(subject.position, destinantion) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, destinantion, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = destinantion;
    }

}
