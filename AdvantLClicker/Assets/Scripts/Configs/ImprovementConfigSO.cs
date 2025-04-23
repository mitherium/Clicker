using UnityEngine;

[CreateAssetMenu(fileName = "NewImprovementConfig", menuName = "Configs/ImprovementConfig")]
public class ImprovementConfigSO : ScriptableObject
{
    public string Name; // Название улучшения
    public int Cost; // Стоимость покупки
    public float IncomeMultiplier; // Множитель дохода в процентах (например, 0.5f = +50%)
}