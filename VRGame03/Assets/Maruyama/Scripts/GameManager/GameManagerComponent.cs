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

    private List<DissolveFadeSprite> m_dissolveFadeSprites;     //ディゾブルテクスチャ

    private List<ClearUI> m_clearUIs = new List<ClearUI>();

    private List<TesterMover> m_movers = new List<TesterMover>();
    private List<FPSController> m_fpsController = new List<FPSController>();

    protected override void Awake()
    {
        base.Awake();
        ChangeState(GameState.Game);

        m_dissolveFadeSprites = new List<DissolveFadeSprite>(FindObjectsOfType<DissolveFadeSprite>());
        m_clearUIs = new List<ClearUI>(FindObjectsOfType<ClearUI>());
        m_movers = new List<TesterMover>(FindObjectsOfType<TesterMover>());
        m_fpsController = new List<FPSController>(FindObjectsOfType<FPSController>());
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
            GameState.Clear => ClearStart,
            GameState.GameOver => GameOver_Start,
            _ => null
        };

        function?.Invoke();
    }

    private void GameOver_Start()
    {
        foreach(var sprite in m_dissolveFadeSprites)
        {
            sprite.FadeStart(FadeObject.FadeType.FadeOut);
        }

        foreach (var mover in m_movers)
        {
            mover.enabled = false;
        }

        foreach (var controller in m_fpsController)
        {
            controller.enabled = false;
        }
    }

    private void ClearStart()
    {
        foreach (var ui in m_clearUIs) {
            ui.ClearEvent();
        }

        foreach (var mover in m_movers)
        {
            mover.enabled = false;
        }

        foreach (var controller in m_fpsController)
        {
            controller.enabled = false;
        }

    }
}
