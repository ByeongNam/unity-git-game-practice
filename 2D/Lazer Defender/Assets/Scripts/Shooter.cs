using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 5f;
    [SerializeField] float firingRate = 0.3f;
    [Header("AI")]
    [SerializeField] float firingVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [SerializeField] bool useAI;
    [SerializeField] bool isBoss = false;

    [HideInInspector] public bool isFiring;
    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;
    private void Awake() {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    void Start()
    {
        if(useAI){
            isFiring = true;
        }
    }
    void Update()
    {
        Fire();
    }
    void Fire(){
        if(isFiring && firingCoroutine == null){
            firingCoroutine = StartCoroutine(FireCountinuously());
        }
        else if(!isFiring && firingCoroutine != null){
            StopCoroutine(firingCoroutine);
            firingCoroutine =null;
        }
        
    }

    IEnumerator FireCountinuously(){
        while(true){
            GameObject instance = Instantiate(projectilePrefab, 
                                            transform.position,
                                            Quaternion.identity);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if(rb != null){
                rb.velocity = transform.up * projectileSpeed;
            }
            Destroy(instance, projectileLifeTime);   

            audioPlayer.PlayShootingClip();
            Debug.Log(transform.up);

            if(isBoss){
                instance = Instantiate(projectilePrefab, 
                                            transform.position,
                                            Quaternion.Euler(0,0,-30));
                rb = instance.GetComponent<Rigidbody2D>();                    
                if(rb != null){
                    rb.velocity = new Vector3(-0.5f,-0.866f,0) * projectileSpeed;
                }
                Destroy(instance, projectileLifeTime);  
                instance = Instantiate(projectilePrefab, 
                                            transform.position,
                                            Quaternion.Euler(0,0,30));
                rb = instance.GetComponent<Rigidbody2D>();                    
                if(rb != null){
                    rb.velocity = new Vector3(0.5f,-0.866f,0) * projectileSpeed;
                }
                Destroy(instance, projectileLifeTime);
            }

            yield return new WaitForSeconds(GetRandomFiringRate());
        }
    }

    float GetRandomFiringRate(){
        float timeToNextProjectile = Random.Range(firingRate - firingVariance,
                                    firingRate + firingVariance);
        return Mathf.Clamp(timeToNextProjectile,minimumFiringRate,float.MaxValue);
    }
}
