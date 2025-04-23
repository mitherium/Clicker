using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText; // Текст для отображения баланса

    private EcsWorld _world;

    public void Initialize(EcsWorld world)
    {
        _world = world;
    }

    public void UpdateUI()
    {
        if (_balanceText == null) return;

        var playerFilter = _world.Filter<PlayerComponent>().End();
        var playerPool = _world.GetPool<PlayerComponent>();

        foreach (var playerEntity in playerFilter)
        {
            ref var player = ref playerPool.Get(playerEntity);
            _balanceText.text = $"Баланс: {player.Balance}$";
        }
    }

    private void Update()
    {
        UpdateUI(); // Обновляем UI каждый кадр
    }
}