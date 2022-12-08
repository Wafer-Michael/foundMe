using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComponent : SingletonMonoBehaviour<GameManagerComponent>
{
    public enum GameState { 
        Reserve,    //準備
        Game,       //ゲーム
        GameOver,   //ゲームオーバー
        Clear,      //クリア
    }

    private GameState m_currentState = GameState.Reserve;

    public GameState CurrentState => m_currentState;


    protected override void Awake()
    {
        base.Awake();
        ChangeState(GameState.Game);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ChangeState(GameState state)
    {
        //同じステートなら変更処理をしない。
        if (state == m_currentState) {
            return;
        }
        
        m_currentState = state; //ステートの変更

        //変更時に一度だけ呼びたい処理を記述。
        System.Action function = state switch
        {
            GameState.Reserve => null,
            GameState.Game => null,
            GameState.GameOver => GameOver_Start,
            _ => null
        };

        function?.Invoke();
    }

    private void GameOver_Start()
    {

    }
}
