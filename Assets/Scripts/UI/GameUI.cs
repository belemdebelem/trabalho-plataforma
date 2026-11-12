using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Atualiza o contador de moedas e exibe as mensagens de fim de partida.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("Referencias")]
    public Text coinText;
    public GameObject endPanel;
    public Text endMessage;
    public Button restartButton;

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            UpdateCoins(GameManager.Instance.Coins);
        }
        else
        {
            UpdateCoins(0);
        }
    }

    public void UpdateCoins(int coins)
    {
        if (coinText != null)
        {
            coinText.text = "MOEDAS: " + coins;
        }
    }

    public void ShowVictory(int coins)
    {
        ShowEndMessage("FASE CONCLUÍDA!\nMOEDAS: " + coins);
    }

    public void ShowDefeat(int coins)
    {
        ShowEndMessage("VOCÊ CAIU!\nMOEDAS: " + coins);
    }

    private void ShowEndMessage(string message)
    {
        if (endMessage != null)
        {
            endMessage.text = message;
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }
    }

    private void OnRestartClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
}
