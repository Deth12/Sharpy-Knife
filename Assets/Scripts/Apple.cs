using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] hitEffects;
    [SerializeField]
    private ParticleSystem hitApple;
    
    // FOR DEBUG
    public int appleValue;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Knife")
        {
            HitParticle(transform.position);
            SoundManager.instance.PlayOneShot(hitEffects[Random.Range(0, hitEffects.Length)]);
            GameStatus.Apples += appleValue;
            Destroy (gameObject);
        }
    }

    private void HitParticle(Vector3 position)
    {
        ParticleSystem p = Instantiate(hitApple);
        p.transform.position = position;
        p.Play();
    }
}
