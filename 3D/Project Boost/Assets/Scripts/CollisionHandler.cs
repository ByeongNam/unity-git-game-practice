using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    Movement movement;
    AudioSource audioSource;
    [Header("AudioClip")]
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip success;
    [Header("Particle Effect")]
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    bool isTransitioning = false; // Object State
    bool collisionDisable = false; // Cheat mode
    void Start()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }
    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L)){
            Debug.Log("Load Next");
            LoadNextSceneWithoutDelay();
        }
        else if(Input.GetKeyDown(KeyCode.C)){
            collisionDisable = !collisionDisable; // toggle collision
            Debug.Log("No Collision: " + collisionDisable);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisable) { return; } //알아보기 쉽게 

        switch (other.gameObject.tag)
        {
            case "Finish":
                StartLandingSequence();
                /* vs Invoke(LoadNextScene , 2) -> 
                coroutine은 파라미터 전달가능 
                invoke 는 gameobject가 off여도 실행 가능 */
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            case "Friendly":
                Debug.Log("Friendly");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    void LoadNextSceneWithoutDelay()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(2);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void StartCrashSequence()
    {
        isTransitioning = true; // state 변화
        audioSource.Stop(); // 재생되고 있는 audio 해제
        audioSource.PlayOneShot(explosion); // SFX
        crashParticles.Play(); //particle effect
        GetComponent<Movement>().enabled = false;

        StartCoroutine(Die());
    }
    void StartLandingSequence()
    {
        isTransitioning = true; // state 변화
        audioSource.Stop(); // 재생되고 있는 audio 해제
        audioSource.PlayOneShot(success); // SFX
        successParticles.Play(); // particle effect
        GetComponent<Rigidbody>().isKinematic = true; // 로켓이 그자리에서 멈추게 하기위해 사용
        GetComponent<Movement>().enabled = false;
        StartCoroutine(LoadNextScene());

    }
    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(2);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);// 현재의 scene을 로드
    }
}
