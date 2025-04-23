using UnityEngine;

[CreateAssetMenu(fileName = "BusinessNames", menuName = "Configs/BusinessNames")]
public class BusinessNamesSO : ScriptableObject
{
    public string[] BusinessNames; // Названия бизнесов
    public string[] ImprovementNames1; // Названия улучшений 1
    public string[] ImprovementNames2; // Названия улучшений 2
}