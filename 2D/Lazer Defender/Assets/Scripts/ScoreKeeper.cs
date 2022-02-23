using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    static ScoreKeeper instance;
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
    private int currentScore;
    public int GetScore(){
        return currentScore;
    }
    public void AddScore(int value){
        currentScore += value;
        Mathf.Clamp(currentScore, 0, int.MaxValue);
        Debug.Log(currentScore);
    }
    public void ResetScore(){
        currentScore = 0;
    }
    
}
