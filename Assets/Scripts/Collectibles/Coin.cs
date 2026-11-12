using UnityEngine;

/// <summary>
/// Moeda coletavel. Utiliza um collider do tipo Trigger e comunica a coleta
/// ao GameManager, garantindo que cada moeda seja coletada apenas uma vez.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    private bool collected;

    private void Reset()
    {
        collected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        collected = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin();
        }

        Destroy(gameObject);
    }
}
