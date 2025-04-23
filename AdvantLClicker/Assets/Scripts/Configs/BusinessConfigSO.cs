using UnityEngine;

[CreateAssetMenu(fileName = "BusinessConfig", menuName = "Configs/BusinessConfig")]
public class BusinessConfigSO : ScriptableObject
{
    public string Name; // Название бизнеса
    public int BaseCost; // Базовая стоимость покупки бизнеса
    public int BaseIncome; // Базовый доход
    public float DelayIncome; // Задержка между доходами (в секундах)

    public ImprovementConfig Improvement1; // Первое улучшение
    public ImprovementConfig Improvement2; // Второе улучшение
}

[System.Serializable]
public class ImprovementConfig
{
    public string Name; // Название улучшения
    public int Cost; // Стоимость улучшения
    public float IncomeMultiplier; // Множитель дохода
}