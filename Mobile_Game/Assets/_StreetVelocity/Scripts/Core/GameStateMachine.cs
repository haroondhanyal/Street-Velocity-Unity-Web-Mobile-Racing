using System;

namespace StreetVelocity.Core
{
    public sealed class GameStateMachine
    {
        public GameState Current { get; private set; } = GameState.Boot;
        public event Action<GameState, GameState> Changed;

        public void TransitionTo(GameState next)
        {
            if (Current == next) return;
            var previous = Current;
            Current = next;
            Changed?.Invoke(previous, next);
        }
    }
}
