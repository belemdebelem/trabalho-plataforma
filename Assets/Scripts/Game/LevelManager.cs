using UnityEngine;

/// <summary>
/// Controla os limites da fase e a area de chegada.
/// Deve ser anexado ao objeto com o collider Trigger da chegada.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Limites da fase")]
    [Tooltip("Altura minima permitida. Abaixo dela, a partida e considerada perdida.")]
    public float fallThresholdY = -10f;

    private bool fallDetected;

    private void Update()
    {
        if (fallDetected || player == null)
        {
            return;
        }

        if (player.position.y < fallThresholdY)
        {
            fallDetected = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.Lose();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Win();
        }
    }
}
