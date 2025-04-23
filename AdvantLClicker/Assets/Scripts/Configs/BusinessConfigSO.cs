using UnityEngine;

[CreateAssetMenu(fileName = "BusinessConfig", menuName = "Configs/BusinessConfig")]
public class BusinessConfigSO : ScriptableObject
{
    public string Name; // �������� �������
    public int BaseCost; // ������� ��������� ������� �������
    public int BaseIncome; // ������� �����
    public float DelayIncome; // �������� ����� �������� (� ��������)

    public ImprovementConfig Improvement1; // ������ ���������
    public ImprovementConfig Improvement2; // ������ ���������
}

[System.Serializable]
public class ImprovementConfig
{
    public string Name; // �������� ���������
    public int Cost; // ��������� ���������
    public float IncomeMultiplier; // ��������� ������
}