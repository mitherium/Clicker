using UnityEngine;

[CreateAssetMenu(fileName = "NewImprovementConfig", menuName = "Configs/ImprovementConfig")]
public class ImprovementConfigSO : ScriptableObject
{
    public string Name; // �������� ���������
    public int Cost; // ��������� �������
    public float IncomeMultiplier; // ��������� ������ � ��������� (��������, 0.5f = +50%)
}