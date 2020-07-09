using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KnifeHit/LevelTemplates")]
public class LevelTemplates : ScriptableObject
{
    public List<LevelPattern> easyLevelPatterns = new List<LevelPattern>();
    public List<LevelPattern> mediumLevelPatterns = new List<LevelPattern>();
    public List<LevelPattern> hardLevelPatterns = new List<LevelPattern>();

    public List<RotationPattern> easyRotationPatterns = new List<RotationPattern>();
    public List<RotationPattern> mediumRotationPatterns = new List<RotationPattern>();
    public List<RotationPattern> hardRotationPatterns = new List<RotationPattern>();

}

[System.Serializable]
public class RotationPattern
{
    public List<RotationStep> rotationSteps = new List<RotationStep>();
}

[System.Serializable]
public class RotationStep
{
    public float time = 0f;
    [Range(-720,720)]
    public float z = 0f;
    public AnimationCurve curve;
}

[System.Serializable]
public class LevelPattern
{
    [Range(0, 1)] 
    public float appleSpawnChance;

    public int enemyLives;
    
    public List<float> appleSpawnAngles = new List<float>();
    public List<float> knifeSpawnAngles = new List<float>();
}