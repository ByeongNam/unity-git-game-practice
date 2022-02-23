using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip shootingClip;
    [SerializeField] [Range(0f,1f)] float shootingVolume = 1f;
    [SerializeField] AudioClip damageClip;
    [SerializeField] [Range(0f,1f)] float damageVolume = 1f;
    static AudioPlayer instance;
    void Awake() {
        ManageSingleton();
    }
    void ManageSingleton(){
        if(instance != null){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayShootingClip(){
        PlayClip(shootingClip, shootingVolume);
    }

    public void PlayDamageClip(){
        PlayClip(damageClip, damageVolume);
    }

    void PlayClip(AudioClip audioClip, float volume){
        if(audioClip != null){
            AudioSource.PlayClipAtPoint(audioClip,
                                        Camera.main.transform.position,
                                        volume);
        }
    }
}
