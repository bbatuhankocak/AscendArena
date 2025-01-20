using UnityEngine;

public class PlayerStateMachine 
{

    public PlayerState currentState {  get; private set; }


    public void Initialize(PlayerState _playerState)
    {
        this.currentState = _playerState;
        currentState.Enter();
        
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        this.currentState = _newState;
        currentState.Enter();
    }
    
}
