using Leopotam.EcsLite;
using UnityEngine;

public sealed class LoadSaveSystem : IEcsInitSystem, IEcsRunSystem
{
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        // Загружаем игрока
        var playerFilter = world.Filter<PlayerComponent>().End();
        var playerPool = world.GetPool<PlayerComponent>();

        foreach (var playerEntity in playerFilter)
        {
            ref var player = ref playerPool.Get(playerEntity);
            player.Balance = PlayerPrefs.GetInt("Player_Balance", 150); // Начальный баланс

            // Нет необходимости вызывать Replace, данные обновляются напрямую через ref
        }

        // Загружаем бизнесы
        var businessFilter = world.Filter<BusinessComponent>().End();
        var businessPool = world.GetPool<BusinessComponent>();

        foreach (var businessEntity in businessFilter)
        {
            ref var business = ref businessPool.Get(businessEntity);

            business.Level = PlayerPrefs.GetInt($"Business_{businessEntity}_Level", 0);
            business.IsPurchased = PlayerPrefs.GetInt($"Business_{businessEntity}_IsPurchased", 0) == 1;
            business.Improvement1Bought = PlayerPrefs.GetInt($"Business_{businessEntity}_Improvement1Bought", 0) == 1;
            business.Improvement2Bought = PlayerPrefs.GetInt($"Business_{businessEntity}_Improvement2Bought", 0) == 1;
            business.Progress = PlayerPrefs.GetFloat($"Business_{businessEntity}_Progress", 0f);

            // Нет необходимости вызывать Replace, данные обновляются напрямую через ref
        }
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        // Сохраняем игрока
        var playerFilter = world.Filter<PlayerComponent>().End();
        var playerPool = world.GetPool<PlayerComponent>();

        foreach (var playerEntity in playerFilter)
        {
            ref var player = ref playerPool.Get(playerEntity);
            PlayerPrefs.SetInt("Player_Balance", player.Balance);
        }

        // Сохраняем бизнесы
        var businessFilter = world.Filter<BusinessComponent>().End();
        var businessPool = world.GetPool<BusinessComponent>();

        foreach (var businessEntity in businessFilter)
        {
            ref var business = ref businessPool.Get(businessEntity);

            PlayerPrefs.SetInt($"Business_{businessEntity}_Level", business.Level);
            PlayerPrefs.SetInt($"Business_{businessEntity}_IsPurchased", business.IsPurchased ? 1 : 0);
            PlayerPrefs.SetInt($"Business_{businessEntity}_Improvement1Bought", business.Improvement1Bought ? 1 : 0);
            PlayerPrefs.SetInt($"Business_{businessEntity}_Improvement2Bought", business.Improvement2Bought ? 1 : 0);
            PlayerPrefs.SetFloat($"Business_{businessEntity}_Progress", business.Progress);
        }

        PlayerPrefs.Save();
    }
}