using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Setting }
public class GameManager : Singleton<GameManager>
{
    private GameState gameState;

    private void Start()
    {
        UIManager.Instance.OpenUI<MainMenu>();
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState) => this.gameState == gameState;



}
