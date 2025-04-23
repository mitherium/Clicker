using UnityEngine;

[CreateAssetMenu(fileName = "BusinessNames", menuName = "Configs/BusinessNames")]
public class BusinessNamesSO : ScriptableObject
{
    public string[] BusinessNames; // �������� ��������
    public string[] ImprovementNames1; // �������� ��������� 1
    public string[] ImprovementNames2; // �������� ��������� 2
}