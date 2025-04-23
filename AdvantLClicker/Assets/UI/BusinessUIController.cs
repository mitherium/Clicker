using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUIController : MonoBehaviour
{
    // Ссылки на UI-элементы
    [SerializeField] private TextMeshProUGUI _nameText; // Текст названия бизнеса
    [SerializeField] private Slider _progressSlider; // Слайдер прогресса дохода
    [SerializeField] private TextMeshProUGUI _levelText; // Текст уровня
    [SerializeField] private TextMeshProUGUI _incomeText; // Текст дохода
    [SerializeField] private Button _levelUpButton; // Кнопка покупки уровня
    [SerializeField] private Button _improvement1Button; // Кнопка первого улучшения
    [SerializeField] private Button _improvement2Button; // Кнопка второго улучшения

    // Данные ECS
    private EcsWorld _world;
    private int _entityId;

    public void Initialize(EcsWorld world, int entityId)
    {
        _world = world;
        _entityId = entityId;
    }

    private void Update()
    {
        if (_world == null || _entityId == 0) return;

        var businessPool = _world.GetPool<BusinessComponent>();
        var configPool = _world.GetPool<Ref<BusinessConfigSO>>();

        if (businessPool.Has(_entityId) && configPool.Has(_entityId))
        {
            ref var business = ref businessPool.Get(_entityId);
            ref var config = ref configPool.Get(_entityId).Value;

            UpdateUI(business, config); // Обновляем UI каждый кадр
        }
    }

    public void UpdateUI(BusinessComponent business, BusinessConfigSO config)
    {
        // Обновляем название бизнеса
        if (_nameText != null)
            _nameText.text = config.Name;

        // Обновляем прогресс слайдера
        if (_progressSlider != null)
            _progressSlider.value = business.Progress;

        // Обновляем уровень
        if (_levelText != null)
            _levelText.text = $"LVL {business.Level}";

        // Обновляем доход
        if (_incomeText != null)
            _incomeText.text = $"Доход: {CalculateIncome(business, config)}$";

        // Настройка кнопки покупки уровня
        if (_levelUpButton != null)
        {
            var playerFilter = _world.Filter<PlayerComponent>().End();
            var playerPool = _world.GetPool<PlayerComponent>();

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerPool.Get(playerEntity);

                int cost = business.IsPurchased ? (business.Level + 1) * config.BaseCost : config.BaseCost;

                if (player.Balance >= cost)
                {
                    _levelUpButton.interactable = true;
                    _levelUpButton.GetComponentInChildren<TextMeshProUGUI>().text = $"LVL UP\nЦена: {cost}$";
                }
                else
                {
                    _levelUpButton.interactable = false;
                    _levelUpButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Купить\nЦена: {cost}$";
                }
            }
        }

        // Настройка кнопки первого улучшения
        if (_improvement1Button != null)
        {
            var playerFilter = _world.Filter<PlayerComponent>().End();
            var playerPool = _world.GetPool<PlayerComponent>();

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerPool.Get(playerEntity);

                if (!business.Improvement1Bought && player.Balance >= config.Improvement1.Cost)
                {
                    _improvement1Button.interactable = true;
                    _improvement1Button.GetComponentInChildren<TextMeshProUGUI>().text = $"{config.Improvement1.Name}\nЦена: {config.Improvement1.Cost}$";
                }
                else
                {
                    _improvement1Button.interactable = false;
                    _improvement1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Куплено";
                }
            }
        }

        // Настройка кнопки второго улучшения
        if (_improvement2Button != null)
        {
            var playerFilter = _world.Filter<PlayerComponent>().End();
            var playerPool = _world.GetPool<PlayerComponent>();

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerPool.Get(playerEntity);

                if (!business.Improvement2Bought && player.Balance >= config.Improvement2.Cost)
                {
                    _improvement2Button.interactable = true;
                    _improvement2Button.GetComponentInChildren<TextMeshProUGUI>().text = $"{config.Improvement2.Name}\nЦена: {config.Improvement2.Cost}$";
                }
                else
                {
                    _improvement2Button.interactable = false;
                    _improvement2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Куплено";
                }
            }
        }
    }

    private int CalculateIncome(BusinessComponent business, BusinessConfigSO config)
    {
        int baseIncome = config.BaseIncome * business.Level;
        float multiplier = 1f;

        if (business.Improvement1Bought)
            multiplier += config.Improvement1.IncomeMultiplier;

        if (business.Improvement2Bought)
            multiplier += config.Improvement2.IncomeMultiplier;

        return (int)(baseIncome * multiplier);
    }

    public void OnLevelUpButtonClick()
    {
        if (_world == null || _entityId == 0) return;

        var businessPool = _world.GetPool<BusinessComponent>();
        var configPool = _world.GetPool<Ref<BusinessConfigSO>>();
        var playerFilter = _world.Filter<PlayerComponent>().End();
        var playerPool = _world.GetPool<PlayerComponent>();

        if (businessPool.Has(_entityId) && configPool.Has(_entityId))
        {
            ref var business = ref businessPool.Get(_entityId);
            ref var config = ref configPool.Get(_entityId).Value;

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerPool.Get(playerEntity);

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

                    UpdateUI(business, config); // Обновляем UI
                    Debug.Log($"Уровень куплен для бизнеса ID {_entityId}");
                }
            }
        }
    }

    public void OnImprovement1ButtonClick()
    {
        if (_world == null || _entityId == 0) return;

        var businessPool = _world.GetPool<BusinessComponent>();
        var configPool = _world.GetPool<Ref<BusinessConfigSO>>();
        var playerFilter = _world.Filter<PlayerComponent>().End();
        var playerPool = _world.GetPool<PlayerComponent>();

        if (businessPool.Has(_entityId) && configPool.Has(_entityId))
        {
            ref var business = ref businessPool.Get(_entityId);
            ref var config = ref configPool.Get(_entityId).Value;

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerPool.Get(playerEntity);

                if (!business.Improvement1Bought && player.Balance >= config.Improvement1.Cost)
                {
                    player.Balance -= config.Improvement1.Cost;
                    business.Improvement1Bought = true;

                    UpdateUI(business, config); // Обновляем UI
                    Debug.Log($"Первое улучшение куплено для бизнеса ID {_entityId}");
                }
            }
        }
    }

    public void OnImprovement2ButtonClick()
    {
        if (_world == null || _entityId == 0) return;

        var businessPool = _world.GetPool<BusinessComponent>();
        var configPool = _world.GetPool<Ref<BusinessConfigSO>>();
        var playerFilter = _world.Filter<PlayerComponent>().End();
        var playerPool = _world.GetPool<PlayerComponent>();

        if (businessPool.Has(_entityId) && configPool.Has(_entityId))
        {
            ref var business = ref businessPool.Get(_entityId);
            ref var config = ref configPool.Get(_entityId).Value;

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerPool.Get(playerEntity);

                if (!business.Improvement2Bought && player.Balance >= config.Improvement2.Cost)
                {
                    player.Balance -= config.Improvement2.Cost;
                    business.Improvement2Bought = true;

                    UpdateUI(business, config); // Обновляем UI
                    Debug.Log($"Второе улучшение куплено для бизнеса ID {_entityId}");
                }
            }
        }
    }
}