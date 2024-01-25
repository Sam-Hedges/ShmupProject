using UnityEngine;

public class Idle : BaseState<EPlayerState>
{
    private PlayerController playerController;

    public Idle(PlayerController playerController) : base(EPlayerState.Idle)
    {
        this.playerController = playerController;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }
    
    public override EPlayerState GetNextState()
    {
        // If movement input is detected, transition to Moving
        if (playerController.isMovementPressed) 
        {
            return EPlayerState.Moving;
        }
        
        return StateKey;
    }
    
    
    
}