using Leopotam.EcsLite;
using UnityEngine;

public sealed class BusinessSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        // �������� ������ � ����
        var businessFilter = world.Filter<BusinessComponent>().Inc<Ref<BusinessConfigSO>>().End();
        var businessPool = world.GetPool<BusinessComponent>();
        var configPool = world.GetPool<Ref<BusinessConfigSO>>();
        var playerFilter = world.Filter<PlayerComponent>().End();
        var playerPool = world.GetPool<PlayerComponent>();

        foreach (var businessEntity in businessFilter)
        {
            ref var business = ref businessPool.Get(businessEntity);
            ref var config = ref configPool.Get(businessEntity).Value;

            // ���������, ������ �� ������
            if (business.IsPurchased)
            {
                // ��������� �������� ������
                float progressDelta = Time.deltaTime / config.DelayIncome;
                business.Progress += progressDelta;

                // ���������, ������ �� ���������� ������� ��� ��������� ������
                if (business.Progress >= 1f)
                {
                    business.Progress = 0f; // ���������� ��������

                    // ��������� ����� ������
                    foreach (var playerEntity in playerFilter)
                    {
                        ref var player = ref playerPool.Get(playerEntity);

                        int income = config.BaseIncome * business.Level;

                        // ��������� ��������� ������ �� ���������
                        float multiplier = 1f;
                        if (business.Improvement1Bought)
                            multiplier += config.Improvement1.IncomeMultiplier;

                        if (business.Improvement2Bought)
                            multiplier += config.Improvement2.IncomeMultiplier;

                        player.Balance += (int)(income * multiplier);
                    }
                }
            }
        }
    }
}