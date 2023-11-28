using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    public Level[] levels;

    private Level currentLevel;

    private int levelIndex = 0;
    private Transform startPoint;

    public List<Enemy> Enemies { get => enemies;}
    public Player Player { get => player;}
    public int LevelIndex { get => levelIndex;  }
    public Transform StartPoint { get => startPoint;}

    private int currentRound = 0;
    private int[] enemiesPerRound;
    private PoolType[] enemyTypesPerRoundArray;

    void Update() 
    {
        if(GameManager.Instance.IsState(GameState.GamePlay))
        {
            CreateNewWaveOfEnemies();
        }
    
    }

    private void OnInit()
    {
        startPoint = currentLevel.startPoint;
        currentRound = 0;
        enemiesPerRound = currentLevel.levelData.enemiesPerRound;
        enemyTypesPerRoundArray = currentLevel.levelData.enemyTypesPerRoundArray;

        player.OnInit();

        for(int i =0; i<enemies.Count; i++)
        {
            enemies[i].OnInit();
        }
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void OnStartGame()
    {
        LoadLevel(0);
        OnInit();
        GameManager.Instance.ChangeState(GameState.GamePlay);
        
    }

    public void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (level < levels.Length)
        {
            currentLevel = Instantiate(levels[level]);
        }
        else
        {
            //hetlevel
            MainMenu();
        }

    }

    internal void OnNextLevel()
    {
        levelIndex++;

        LoadLevel(levelIndex);
        OnInit();
    }

    private void CreateMultipleEnemies<T>(int count, PoolType poolType) where T : Enemy
    {
        for (int i = 0; i < count; i++)
        {
            T enemy = SimplePool.Spawn<T>(poolType, currentLevel.GetRandomPointInPatrolArea(), Quaternion.identity);
            enemy.OnInit();
            enemies.Add(enemy);
        }
    }


    private void CreateNewWaveOfEnemies()
    {
        if (enemies.Count == 0)
        {
            enemies.Clear();
            currentRound++;
            if (currentRound <= enemiesPerRound.Length)
            {
                int count = enemiesPerRound[currentRound - 1]; 
                for(int i = 0; i< count; i++)
                {
                    PoolType enemyType = GetRandomEnemyType(); 
                    CreateMultipleEnemies<Enemy>(1, enemyType);
                }
                
            }
            else
            {
                currentLevel.SetActiveEndPoint();
            }
        }
    }

    private PoolType GetRandomEnemyType()
    {
        return enemyTypesPerRoundArray[Random.Range(0, enemyTypesPerRoundArray.Length)];
    }

    public void Fall()
    {
        GameManager.Instance.ChangeState(GameState.Finish);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<Fail>();
        foreach(Enemy enemy in enemies)
        {
            enemy.StopMoving();
        }
    }

    public void ClearEnemy()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.OnDeath();
        }
        enemies.Clear();
    }
    


    public void OnRetry()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenu>();
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        levelIndex = 0;
    }

    public void MainMenu()
    {
        UIManager.Instance.CloseAll();
        GameManager.Instance.ChangeState(GameState.MainMenu);
        ClearEnemy();
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        levelIndex = 0;
        player.OnDeath();
    }


}
