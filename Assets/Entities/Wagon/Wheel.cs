using UnityEngine;

[CreateAssetMenu(fileName = "New Wheel", menuName = "WagonParts/Wheel")]
public class Wheel : WagonPart
{
    [SerializeField] private float _qualityModifier;
    public float QualityModifier => _qualityModifier;
}
