using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {    
        if(GameStatus.Sound)
            source.PlayOneShot(clip, volume);
    }

    public void PlayVibration()
    {
        if(GameStatus.Vibration)
            Handheld.Vibrate ();    
    }
    
}
