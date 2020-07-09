using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private ParticleSystem hitEffect;
    [SerializeField]
    private ParticleSystem destroyEffect;

    private List<Knife> hittedKnives = new List<Knife>();
    
    public float spawnHeight = 0.5f;
    
    private int _lives;
    public int Lives
    {
        get => _lives;
        set => _lives = value;
    }

    private RotationPattern rotationPattern;
    private int rotIndex = 0;


    public void SetupEnemy(RotationPattern rp, LevelPattern lp)
    {
        Lives = lp.enemyLives;
        Populate(lp);
        rotationPattern = rp;
        ApplyRotationPattern();
    }

    private void ApplyRotationPattern()
    {
        rotIndex = (rotIndex + 1) % rotationPattern.rotationSteps.Count;
        LeanTween.rotateZ (gameObject, transform.localRotation.eulerAngles.z + 
                                       rotationPattern.rotationSteps[rotIndex].z, rotationPattern.rotationSteps[rotIndex].time)
            .setOnComplete (ApplyRotationPattern).setEase (rotationPattern.rotationSteps[rotIndex].curve);
    }

    private void Populate(LevelPattern lp)
    {
        PopulateKnives(lp);
        PopulateApples(lp);
    }

    private void PopulateKnives(LevelPattern lp)
    {
        foreach (var angle in lp.knifeSpawnAngles)
        {
            // Spawn Knives
            GameObject knifeObj = Instantiate(GameManager.instance.knifeObject, transform);
            Knife knife = knifeObj.GetComponent<Knife>();
            knife.IsThrown = true;
            knife.IsHitted = true;
            // TOFIX GET COMPONENT
            CalculateCorrectPosition(knifeObj.transform,GetComponent<CircleCollider2D>(), angle, 90f);
            //knifeObj.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
        }
    }

    private void PopulateApples(LevelPattern lp)
    {
        if (lp.appleSpawnChance < Random.Range(0f, 1f))
            return;
        foreach (var angle in lp.appleSpawnAngles)
        {
            GameObject appleObj = Instantiate(GameManager.instance.appleObject, this.transform, true);
            CalculateCorrectPosition(appleObj.transform, GetComponent<CircleCollider2D>(), angle, -90f);
        }
    }
    
    private void CalculateCorrectPosition(Transform item, CircleCollider2D col, float angle, float objAngleOffset)
    {
        angle = angle + 90f;
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        float o = spawnHeight + col.radius;
        Vector2 offset = new Vector2(sin, cos) * o;
        item.localPosition = (Vector2)transform.localPosition + offset;
        item.localRotation = Quaternion.Euler (0, 0, -angle + 90f + objAngleOffset);
    }
    
    public void OnKnifeHit(Knife k)
    {
        if (GameStatus.IsGameOver && k.IsFailed)
            return;
        k.transform.SetParent(this.transform);
        k.IsHitted = true;
        k.rb.isKinematic = true;
        hittedKnives.Add(k);
        SoundManager.instance.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
        HitParticle(k.transform.position);
        GameStatus.Score++;
        LeanTween.moveLocalY (gameObject, 0.1f, 0.05f).setLoopPingPong(1);
        if (hittedKnives.Count >= Lives && !GameStatus.IsGameOver)
            OnDeath();
    }

    private void HitParticle(Vector3 position)
    {
        ParticleSystem p = Instantiate(hitEffect);
        p.transform.position = position;
        p.Play();
    }

    private void OnDeath()
    {
        ParticleSystem p = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        p.Play();
        foreach (var knife in hittedKnives)
        {
           knife.gameObject.transform.parent = null;
           knife.Release();
        }
        Destroy(gameObject);
        GameManager.instance.InitializeNextLevel();
    }

    public void SafeDestroy(float time)
    {
        foreach (Knife knife in hittedKnives)
        {
            if(knife != null)
                Destroy(knife.gameObject);            
        }
        Destroy(gameObject, time);
    }
}

