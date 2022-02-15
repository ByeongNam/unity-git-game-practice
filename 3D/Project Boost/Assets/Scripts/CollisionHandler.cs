using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    Movement movement;
    void Start(){
        movement = GetComponent<Movement>();
    }
    void OnCollisionEnter(Collision other) {
        switch(other.gameObject.tag)
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
            default :
                StartCrashSequence();
                break;
        }    
    }
    IEnumerator LoadNextScene(){
        yield return new WaitForSecondsRealtime(2);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void StartCrashSequence(){
        // todo SFX
        // todo particle effect
        GetComponent<Movement>().enabled = false;
        StartCoroutine(Die());
    }
    void StartLandingSequence(){
        // todo SFX
        // todo particle effect
        GetComponent<Rigidbody>().isKinematic = true; // 로켓이 그자리에서 멈추게 하기위해 사용
        GetComponent<Movement>().enabled = false;
        StartCoroutine(LoadNextScene());
    }
    IEnumerator Die(){
        yield return new WaitForSecondsRealtime(2);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);// 현재의 scene을 로드
    }
}
