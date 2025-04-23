using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUIController : MonoBehaviour
{
    // ������ �� UI-��������
    [SerializeField] private TextMeshProUGUI _nameText; // ����� �������� �������
    [SerializeField] private Slider _progressSlider; // ������� ��������� ������
    [SerializeField] private TextMeshProUGUI _levelText; // ����� ������
    [SerializeField] private TextMeshProUGUI _incomeText; // ����� ������
    [SerializeField] private Button _levelUpButton; // ������ ������� ������
    [SerializeField] private Button _improvement1Button; // ������ ������� ���������
    [SerializeField] private Button _improvement2Button; // ������ ������� ���������

    // ������ ECS
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

            UpdateUI(business, config); // ��������� UI ������ ����
        }
    }

    public void UpdateUI(BusinessComponent business, BusinessConfigSO config)
    {
        // ��������� �������� �������
        if (_nameText != null)
            _nameText.text = config.Name;

        // ��������� �������� ��������
        if (_progressSlider != null)
            _progressSlider.value = business.Progress;

        // ��������� �������
        if (_levelText != null)
            _levelText.text = $"LVL {business.Level}";

        // ��������� �����
        if (_incomeText != null)
            _incomeText.text = $"�����: {CalculateIncome(business, config)}$";

        // ��������� ������ ������� ������
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
                    _levelUpButton.GetComponentInChildren<TextMeshProUGUI>().text = $"LVL UP\n����: {cost}$";
                }
                else
                {
                    _levelUpButton.interactable = false;
                    _levelUpButton.GetComponentInChildren<TextMeshProUGUI>().text = $"������\n����: {cost}$";
                }
            }
        }

        // ��������� ������ ������� ���������
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
                    _improvement1Button.GetComponentInChildren<TextMeshProUGUI>().text = $"{config.Improvement1.Name}\n����: {config.Improvement1.Cost}$";
                }
                else
                {
                    _improvement1Button.interactable = false;
                    _improvement1Button.GetComponentInChildren<TextMeshProUGUI>().text = "�������";
                }
            }
        }

        // ��������� ������ ������� ���������
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
                    _improvement2Button.GetComponentInChildren<TextMeshProUGUI>().text = $"{config.Improvement2.Name}\n����: {config.Improvement2.Cost}$";
                }
                else
                {
                    _improvement2Button.interactable = false;
                    _improvement2Button.GetComponentInChildren<TextMeshProUGUI>().text = "�������";
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

                    UpdateUI(business, config); // ��������� UI
                    Debug.Log($"������� ������ ��� ������� ID {_entityId}");
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

                    UpdateUI(business, config); // ��������� UI
                    Debug.Log($"������ ��������� ������� ��� ������� ID {_entityId}");
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

                    UpdateUI(business, config); // ��������� UI
                    Debug.Log($"������ ��������� ������� ��� ������� ID {_entityId}");
                }
            }
        }
    }
}