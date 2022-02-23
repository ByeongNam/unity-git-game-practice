using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 50;
    [SerializeField] int score = 50;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem smokeEffect;
    ParticleSystem smokeInstance = null;

    [SerializeField] bool applyCameraShake = false;
    CameraShake cameraShake;
    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;
    void Awake() {
        cameraShake = Camera.main.GetComponent<CameraShake>();   
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if(damageDealer != null){
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            ShakeCamera();
            damageDealer.Hit();
        }
    }

    void TakeDamage(int damage){
        health -= damage;
        audioPlayer.PlayDamageClip();
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isPlayer)
        {
            scoreKeeper.AddScore(score);
        }
        else{
            levelManager.LoadGameOver();
        }
        Destroy(gameObject);
        Destroy(smokeInstance);
    }
    
        

    void PlayHitEffect(){
        if(hitEffect != null){
            ParticleSystem instance = Instantiate(hitEffect,transform.position,Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
        if(smokeEffect != null && smokeInstance == null && health <=10){
            smokeInstance = Instantiate(smokeEffect, transform.position, Quaternion.identity,transform);
        }
    }
    void ShakeCamera(){
        if(cameraShake != null && applyCameraShake){
            cameraShake.Play();
        }
    }
    public int GetHealth(){
        return health;
    }
}
