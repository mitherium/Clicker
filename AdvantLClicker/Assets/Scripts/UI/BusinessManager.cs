using Leopotam.EcsLite;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    [SerializeField] private GameObject _businessPanelPrefab; // Префаб панели бизнеса
    [SerializeField] private Transform _contentParent; // Контейнер для панелей

    private EcsWorld _world;

    public void Initialize(EcsWorld world)
    {
        _world = world;

        // Получаем фильтр и пулы
        var businessFilter = _world.Filter<BusinessComponent>().Inc<Ref<BusinessConfigSO>>().End();
        var businessPool = _world.GetPool<BusinessComponent>();
        var configPool = _world.GetPool<Ref<BusinessConfigSO>>();

        // Создаем UI для каждого бизнеса
        foreach (var businessEntity in businessFilter)
        {
            ref var business = ref businessPool.Get(businessEntity);
            ref var configRef = ref configPool.Get(businessEntity);

            // Создаем панель бизнеса
            var panel = Instantiate(_businessPanelPrefab, _contentParent);
            var controller = panel.GetComponent<BusinessUIController>();

            // Инициализируем контроллер
            controller.Initialize(_world, businessEntity);

            // Обновляем UI
            controller.UpdateUI(business, configRef.Value); // Используем configRef.Value
        }
    }
}