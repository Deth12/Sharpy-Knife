using UnityEngine;

public class Knife : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float throwForce = 100f;

    [HideInInspector]
    public Rigidbody2D rb;
    
    [SerializeField]
    private AudioClip[] throwSounds;
    [SerializeField]
    private AudioClip failSound;
    
    private bool _isThrown;
    public bool IsThrown
    {
        get => _isThrown;
        set
        {
            _isThrown = value;
            SwitchColliders(true, false);
        }
    }
    
    private bool _isHitted;
    public bool IsHitted
    {
        get => _isHitted;
        set
        {
            _isHitted = value;
            SwitchColliders(false, true);
        }
    }

    private bool _isFailed;
    public bool IsFailed
    {
        get => _isFailed;
        set
        {
            _isFailed = true;
            SwitchColliders(false, false);
        }
    }

    private BoxCollider2D inHandCollider;
    private BoxCollider2D onEnemyCollider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void SwitchColliders(bool inHand, bool onEnemy)
    {
        if (!inHandCollider || !onEnemyCollider)
        {
            BoxCollider2D[] cols = GetComponentsInChildren<BoxCollider2D>();
            inHandCollider = cols[0];
            onEnemyCollider = cols[1];
        }
        inHandCollider.enabled = inHand;
        onEnemyCollider.enabled = onEnemy;
    }
    
    public void Throw()
    {
        rb.isKinematic = false;
        IsThrown = true;
        SoundManager.instance.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Length)]);
        rb.AddForce(new Vector2(0, throwForce), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Enemy") && !IsFailed)
            col.gameObject.GetComponent<Enemy>().OnKnifeHit(this);
        else if (col.gameObject.tag.Equals("Knife") && !IsHitted)
        {
            IsFailed = true;
            SoundManager.instance.PlayOneShot(failSound);
            rb.freezeRotation = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = Random.Range(20f, 50f) * 25f;
            rb.AddForce(new Vector2(Random.Range(-5f, 5f), -20f), ForceMode2D.Impulse);
            GameManager.instance.Invoke("GameOver", 0.5f);
            //col.gameObject.GetComponent<Knife>().Release();
            //LeanTween.alpha(gameObject, 0f, 0.5f).setOnComplete(() =>
            //{
            //    Destroy(gameObject);
            //});
        }

    }

    public void Release()
    {
        IsFailed = true;
        rb.isKinematic = false;
        rb.gravityScale = 2.5f;
        rb.freezeRotation = false;
        rb.angularVelocity = Random.Range (-20f, 20f) * 35f;
        rb.AddForce (new Vector2 (Random.Range (-10f, 10f), Random.Range (3f, 10f)), ForceMode2D.Impulse);
        GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder + 1;
    }
}
