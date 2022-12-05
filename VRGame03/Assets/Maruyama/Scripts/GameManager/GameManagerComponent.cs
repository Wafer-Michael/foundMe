using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComponent : SingletonMonoBehaviour<GameManagerComponent>
{
    public enum GameState { 
        Reserve,    //����
        Game,       //�Q�[��
        GameOver,   //�Q�[���I�[�o�[
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
        //�����X�e�[�g�Ȃ�ύX���������Ȃ��B
        if (state == m_currentState) {
            return;
        }
        
        m_currentState = state; //�X�e�[�g�̕ύX

        //�ύX���Ɉ�x�����Ăт����������L�q�B
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
