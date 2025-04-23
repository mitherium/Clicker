using Leopotam.EcsLite;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private EcsWorld _world;
    private IEcsSystems _systems;

    void Start()
    {
        // Создаем ECS мир
        _world = new EcsWorld();

        // Создаем системы
        _systems = new EcsSystems(_world);

        // Добавляем системы
        _systems
            .Add(new LoadSaveSystem()) // Загрузка и сохранение прогресса
            .Add(new BusinessSystem()) // Обработка бизнесов
            .Add(new PurchaseSystem()); // Обработка покупок

        // Инициализируем системы
        _systems.Init();

        // Создаем сущности
        CreatePlayer();
        CreateBusinesses();

        // Инициализируем UI
        InitializeUI();
    }

    void Update()
    {
        // Запускаем системы каждый кадр
        _systems.Run();
    }

    void OnDestroy()
    {
        // Очищаем ECS мир при завершении игры
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }

    private void CreatePlayer()
    {
        // Создаем сущность игрока
        var playerEntity = _world.NewEntity();
        var playerPool = _world.GetPool<PlayerComponent>();

        // Добавляем компонент PlayerComponent
        ref var player = ref playerPool.Add(playerEntity);
        player.Balance = 10000; // Начальный баланс
    }

    private void CreateBusinesses()
    {
        // Загружаем конфиги бизнесов из Resources
        var businessConfigs = Resources.LoadAll<BusinessConfigSO>("Configs");

        foreach (var config in businessConfigs)
        {
            // Создаем сущность для каждого бизнеса
            var businessEntity = _world.NewEntity();

            // Добавляем компонент BusinessComponent
            var businessPool = _world.GetPool<BusinessComponent>();
            ref var business = ref businessPool.Add(businessEntity);

            // Инициализируем BusinessComponent
            business.Level = 0;
            business.IsPurchased = false;
            business.Improvement1Bought = false;
            business.Improvement2Bought = false;
            business.Progress = 0f;
            business.LastUpdateTime = Time.time;

            // Добавляем ссылку на конфиг
            var configPool = _world.GetPool<Ref<BusinessConfigSO>>();
            ref var refConfig = ref configPool.Add(businessEntity);
            refConfig.Value = config;
        }
    }

    private void InitializeUI()
    {
        // Находим объект BusinessManager в сцене
        var businessManager = FindObjectOfType<BusinessManager>();

        if (businessManager != null)
        {
            // Передаем ECS мир в BusinessManager
            businessManager.Initialize(_world);

            Debug.Log("BusinessManager успешно инициализирован.");
        }
        else
        {
            Debug.LogError("BusinessManager не найден в сцене!");
        }

        // Находим объект PlayerUI в сцене
        var playerUI = FindObjectOfType<PlayerUI>();

        if (playerUI != null)
        {
            // Передаем ECS мир в PlayerUI
            playerUI.Initialize(_world);

            // Обновляем UI
            playerUI.UpdateUI();

            Debug.Log("PlayerUI успешно инициализирован.");
        }
        else
        {
            Debug.LogError("PlayerUI не найден в сцене!");
        }
    }
}