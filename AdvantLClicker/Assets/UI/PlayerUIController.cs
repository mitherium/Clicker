using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;

    public void UpdateBalance(int balance)
    {
        _balanceText.text = $"Баланс: {balance}$";
    }
}