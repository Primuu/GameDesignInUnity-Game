using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;
    public float starpowerDuration = 10f;

    public int magicMushroomScore = 1000;
    public int starpowerScore = 1000;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                GameManager.Instance.AddScore(magicMushroomScore);
                break;

            case Type.Starpower:
                player.GetComponent<Player>().Starpower(starpowerDuration);
                GameManager.Instance.AddScore(starpowerScore);
                break;
        }

        Destroy(gameObject);
    }

}
