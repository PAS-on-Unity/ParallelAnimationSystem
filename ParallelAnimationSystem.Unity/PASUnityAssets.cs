using UnityEngine;

namespace ParallelAnimationSystem.Unity;

[CreateAssetMenu(fileName = "PASUnityAssets", menuName = "ScriptableObjects/PASUnityAssets", order = 1)]
public class PASUnityAssets : ScriptableObject
{
    public required Material material;
}