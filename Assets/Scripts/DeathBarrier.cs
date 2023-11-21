using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            other.gameObject.SetActive(false);
            AudioManager.Instance.StopSFX();
            GameManager.Instance.ResetLevel(3f);
        } else {
            Destroy(other.gameObject);
        }
    }

}
