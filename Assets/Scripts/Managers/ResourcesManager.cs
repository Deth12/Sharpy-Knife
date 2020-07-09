using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance;

    private void Awake()
    {
        if(instance != null)
            Destroy((gameObject));

        instance = this;
    }


}
