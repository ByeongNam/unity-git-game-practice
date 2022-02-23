using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] float timeBetweenWaves = 3f;
    WaveConfigSO currentWave;
    [SerializeField] bool isLooping = true;
    void Start()
    {
        StartCoroutine(SpawnEnemiesWaves());
        
    }
    public WaveConfigSO GetCurrentWave(){
        return currentWave;
    }
    IEnumerator SpawnEnemiesWaves(){
        do{
            foreach(WaveConfigSO Wave in waveConfigs){
                currentWave = Wave;
                for(int i=0; i<currentWave.GetEnemyCount(); i++){
                    Instantiate(currentWave.GetEnemyPrefab(i),
                                currentWave.GetStartingWaypoint().position,
                                Quaternion.Euler(0,0,180),
                                transform);
                    yield return new WaitForSeconds(currentWave.GetRandomoSpawnTime());
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }while(isLooping);
        
    }
}
