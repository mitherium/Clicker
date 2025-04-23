using Leopotam.EcsLite;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private EcsWorld _world;
    private IEcsSystems _systems;

    void Start()
    {
        // ������� ECS ���
        _world = new EcsWorld();

        // ������� �������
        _systems = new EcsSystems(_world);

        // ��������� �������
        _systems
            .Add(new LoadSaveSystem()) // �������� � ���������� ���������
            .Add(new BusinessSystem()) // ��������� ��������
            .Add(new PurchaseSystem()); // ��������� �������

        // �������������� �������
        _systems.Init();

        // ������� ��������
        CreatePlayer();
        CreateBusinesses();

        // �������������� UI
        InitializeUI();
    }

    void Update()
    {
        // ��������� ������� ������ ����
        _systems.Run();
    }

    void OnDestroy()
    {
        // ������� ECS ��� ��� ���������� ����
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
        // ������� �������� ������
        var playerEntity = _world.NewEntity();
        var playerPool = _world.GetPool<PlayerComponent>();

        // ��������� ��������� PlayerComponent
        ref var player = ref playerPool.Add(playerEntity);
        player.Balance = 10000; // ��������� ������
    }

    private void CreateBusinesses()
    {
        // ��������� ������� �������� �� Resources
        var businessConfigs = Resources.LoadAll<BusinessConfigSO>("Configs");

        foreach (var config in businessConfigs)
        {
            // ������� �������� ��� ������� �������
            var businessEntity = _world.NewEntity();

            // ��������� ��������� BusinessComponent
            var businessPool = _world.GetPool<BusinessComponent>();
            ref var business = ref businessPool.Add(businessEntity);

            // �������������� BusinessComponent
            business.Level = 0;
            business.IsPurchased = false;
            business.Improvement1Bought = false;
            business.Improvement2Bought = false;
            business.Progress = 0f;
            business.LastUpdateTime = Time.time;

            // ��������� ������ �� ������
            var configPool = _world.GetPool<Ref<BusinessConfigSO>>();
            ref var refConfig = ref configPool.Add(businessEntity);
            refConfig.Value = config;
        }
    }

    private void InitializeUI()
    {
        // ������� ������ BusinessManager � �����
        var businessManager = FindObjectOfType<BusinessManager>();

        if (businessManager != null)
        {
            // �������� ECS ��� � BusinessManager
            businessManager.Initialize(_world);

            Debug.Log("BusinessManager ������� ���������������.");
        }
        else
        {
            Debug.LogError("BusinessManager �� ������ � �����!");
        }

        // ������� ������ PlayerUI � �����
        var playerUI = FindObjectOfType<PlayerUI>();

        if (playerUI != null)
        {
            // �������� ECS ��� � PlayerUI
            playerUI.Initialize(_world);

            // ��������� UI
            playerUI.UpdateUI();

            Debug.Log("PlayerUI ������� ���������������.");
        }
        else
        {
            Debug.LogError("PlayerUI �� ������ � �����!");
        }
    }
}