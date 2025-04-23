using Leopotam.EcsLite;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    [SerializeField] private GameObject _businessPanelPrefab; // ������ ������ �������
    [SerializeField] private Transform _contentParent; // ��������� ��� �������

    private EcsWorld _world;

    public void Initialize(EcsWorld world)
    {
        _world = world;

        // �������� ������ � ����
        var businessFilter = _world.Filter<BusinessComponent>().Inc<Ref<BusinessConfigSO>>().End();
        var businessPool = _world.GetPool<BusinessComponent>();
        var configPool = _world.GetPool<Ref<BusinessConfigSO>>();

        // ������� UI ��� ������� �������
        foreach (var businessEntity in businessFilter)
        {
            ref var business = ref businessPool.Get(businessEntity);
            ref var configRef = ref configPool.Get(businessEntity);

            // ������� ������ �������
            var panel = Instantiate(_businessPanelPrefab, _contentParent);
            var controller = panel.GetComponent<BusinessUIController>();

            // �������������� ����������
            controller.Initialize(_world, businessEntity);

            // ��������� UI
            controller.UpdateUI(business, configRef.Value); // ���������� configRef.Value
        }
    }
}