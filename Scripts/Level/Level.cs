using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    public LevelData levelData;
    public Transform startPoint;
    public Transform endPoint;
    public Transform areaBattle;

    private Vector3 patrolAreaCenter;
    private Vector3 patrolAreaSize;

    void Awake() 
    {
        endPoint.gameObject.SetActive(false);    
        LoadLevelData();
    }

    private void LoadLevelData()
    {
        startPoint.position = levelData.playerStartPoint;
        patrolAreaCenter = levelData.patrolAreaCenter1;
        patrolAreaSize = levelData.patrolAreaSize1;
    }


    public void SetActiveEndPoint()
    {
        endPoint.gameObject.SetActive(true);
    }

    

    public void UpdateArea(ref Vector3 patrolAreaCenter, ref Vector3 patrolAreaSize)
    {
        patrolAreaCenter = this.patrolAreaCenter;
        patrolAreaSize =  this.patrolAreaSize;
    }

    public Vector3 GetRandomPointInPatrolArea()
    {
        float randomX = Random.Range(patrolAreaCenter.x - patrolAreaSize.x / 2, patrolAreaCenter.x + patrolAreaSize.x / 2);
        float randomZ = Random.Range(patrolAreaCenter.z - patrolAreaSize.z / 2, patrolAreaCenter.z + patrolAreaSize.z / 2);

        Vector3 randomPoint = new Vector3(randomX, 0f, randomZ);
        return randomPoint;
    }



}
