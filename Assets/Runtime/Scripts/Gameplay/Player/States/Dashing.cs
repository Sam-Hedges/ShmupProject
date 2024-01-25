using UnityEngine;
using System.Collections;
public class Dashing : BaseState<EPlayerState>
{
    private PlayerController playerController;
    
    private float dashStartTime;
    private Vector2 lastMoveDirection;
    private bool isDashing;

    public Dashing(PlayerController playerController) : base(EPlayerState.Dashing)
    {
        this.playerController = playerController;
    }

    public override void Enter()
    {
        // Enables the dash cooldown lock
        isDashing = true;
        
        // Gets the direction of the last movement input
        lastMoveDirection = playerController.movementInputVector;
        
        // Gets the starting time of the dash
        dashStartTime = Time.time;
    }
    
    public override void Update()
    {
        // Clamp Magnitude to 1 ensure all inputs result in the same dash speed
        playerController.Move(Vector2.ClampMagnitude(lastMoveDirection * 1000, 1) );
    }

    public override void Exit()
    {
        // Tells the rest of the script that the dash has finished
        playerController.StartChildCoroutine(DashCooldownTimer(playerController.dashCooldownSeconds));
        while(isDashing) { }
    }

    IEnumerator DashCooldownTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        playerController.isDashing = false;
        isDashing = false;
        yield break;
    }

    
    public override EPlayerState GetNextState()
    {
        // If dash duration is over
        if (Time.time >= dashStartTime + playerController.dashDuration)
        {
            if (playerController.isMovementPressed) 
            {
                return EPlayerState.Moving;
            }
            return EPlayerState.Idle;
        }
        
        return StateKey;
    }
    
}