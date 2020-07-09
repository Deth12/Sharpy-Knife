using UnityEngine;

public class TestRotation : MonoBehaviour
{
    public RotationPattern rotationPattern;
    private int rotIndex = 0;

    private void Start()
    {
        ApplyRotationPattern();
    }
    
    private void ApplyRotationPattern()
    {
        rotIndex = (rotIndex + 1) % rotationPattern.rotationSteps.Count;
        LeanTween.rotateZ (gameObject, transform.localRotation.eulerAngles.z + 
                                       rotationPattern.rotationSteps[rotIndex].z, rotationPattern.rotationSteps[rotIndex].time)
            .setOnComplete (ApplyRotationPattern).setEase (rotationPattern.rotationSteps[rotIndex].curve);
    }

}
