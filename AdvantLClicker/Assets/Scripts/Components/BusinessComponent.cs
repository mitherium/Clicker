using Leopotam.EcsLite;

public struct BusinessComponent
{
    public int Level; // Текущий уровень бизнеса
    public bool IsPurchased; // Куплен ли бизнес
    public bool Improvement1Bought; // Куплено ли первое улучшение
    public bool Improvement2Bought; // Куплено ли второе улучшение
    public float Progress; // Прогресс дохода от 0 до 1
    public float LastUpdateTime; // Время последнего обновления
}