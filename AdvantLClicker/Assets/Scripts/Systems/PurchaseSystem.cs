using Leopotam.EcsLite;
using UnityEngine;

public sealed class PurchaseSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        // �������� ������� � ����
        var playerFilter = world.Filter<PlayerComponent>().End();
        var businessFilter = world.Filter<BusinessComponent>().Inc<Ref<BusinessConfigSO>>().End();
        var playerPool = world.GetPool<PlayerComponent>();
        var businessPool = world.GetPool<BusinessComponent>();
        var configPool = world.GetPool<Ref<BusinessConfigSO>>();

        foreach (var playerEntity in playerFilter)
        {
            ref var player = ref playerPool.Get(playerEntity);

            foreach (var businessEntity in businessFilter)
            {
                ref var business = ref businessPool.Get(businessEntity);
                ref var config = ref configPool.Get(businessEntity).Value;

                // ������� ������ ����� ������� "1"
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    TryBuyLevel(ref player, ref business, config);
                }

                // ������� ������� ��������� ����� ������� "2"
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    TryBuyImprovement1(ref player, ref business, config);
                }

                // ������� ������� ��������� ����� ������� "3"
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    TryBuyImprovement2(ref player, ref business, config);
                }
            }
        }
    }

    private void TryBuyLevel(ref PlayerComponent player, ref BusinessComponent business, BusinessConfigSO config)
    {
        int cost = business.IsPurchased ? (business.Level + 1) * config.BaseCost : config.BaseCost;

        if (player.Balance >= cost)
        {
            player.Balance -= cost;

            if (!business.IsPurchased)
            {
                business.Level = 1;
                business.IsPurchased = true;
            }
            else
            {
                business.Level++;
            }

            Debug.Log($"������� ������. ����� �������: {business.Level}");
        }
        else
        {
            Debug.Log("������������ ������� ��� ������� ������.");
        }
    }

    private void TryBuyImprovement1(ref PlayerComponent player, ref BusinessComponent business, BusinessConfigSO config)
    {
        if (!business.Improvement1Bought && player.Balance >= config.Improvement1.Cost)
        {
            player.Balance -= config.Improvement1.Cost;
            business.Improvement1Bought = true;

            Debug.Log("������ ��������� �������.");
        }
        else
        {
            Debug.Log("������������ ������� ��� ��������� ��� �������.");
        }
    }

    private void TryBuyImprovement2(ref PlayerComponent player, ref BusinessComponent business, BusinessConfigSO config)
    {
        if (!business.Improvement2Bought && player.Balance >= config.Improvement2.Cost)
        {
            player.Balance -= config.Improvement2.Cost;
            business.Improvement2Bought = true;

            Debug.Log("������ ��������� �������.");
        }
        else
        {
            Debug.Log("������������ ������� ��� ��������� ��� �������.");
        }
    }
}