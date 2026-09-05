using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime, GameStateManager manager);
    public abstract void Exit();
}
