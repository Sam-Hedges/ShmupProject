public class Moving : BaseState<EPlayerState>
{
    private PlayerController playerController;

    public Moving(PlayerController playerController) : base(EPlayerState.Moving)
    {
        this.playerController = playerController;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }
    
    public override void Update()
    {
        playerController.Move(playerController.movementInputVector);
    }
    
    public override EPlayerState GetNextState()
    {
        // If the dash input is received while moving
        if (playerController.isDashing)
        {
            return EPlayerState.Dashing;
        }

        // If no movement input is detected, return to Idle
        if (!playerController.isMovementPressed) 
        {
            return EPlayerState.Idle;
        }
        return StateKey;
    }
    
    
    
}