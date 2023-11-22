using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public AudioClip deathSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            other.gameObject.SetActive(false);
            AudioManager.Instance.StopSFX();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(deathSound);
            GameManager.Instance.ResetLevel(3f);
        } else {
            Destroy(other.gameObject);
        }
    }

}
