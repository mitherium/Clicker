using Leopotam.EcsLite;

public struct BusinessComponent
{
    public int Level; // ������� ������� �������
    public bool IsPurchased; // ������ �� ������
    public bool Improvement1Bought; // ������� �� ������ ���������
    public bool Improvement2Bought; // ������� �� ������ ���������
    public float Progress; // �������� ������ �� 0 �� 1
    public float LastUpdateTime; // ����� ���������� ����������
}