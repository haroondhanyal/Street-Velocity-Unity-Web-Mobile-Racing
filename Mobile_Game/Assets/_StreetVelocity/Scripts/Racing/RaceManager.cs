using System;
using StreetVelocity.Vehicle;
using UnityEngine;

namespace StreetVelocity.Racing
{
    public enum RacePhase { Countdown, Racing, Finished }

    /// <summary>Owns the countdown, ordered checkpoint/lap rules, timer, and race finish.</summary>
    public sealed class RaceManager : MonoBehaviour
    {
        [SerializeField] private RaceDefinition definition;
        [SerializeField] private VehicleController vehicle;

        public int CurrentLap { get; private set; } = 1;
        public int NextCheckpoint { get; private set; }
        public bool IsFinished => Phase == RacePhase.Finished;
        public float ElapsedSeconds { get; private set; }
        public float CountdownRemaining { get; private set; }
        public RacePhase Phase { get; private set; } = RacePhase.Countdown;
        public RaceDefinition Definition => definition;
        public int CheckpointsPerLap => definition != null ? definition.checkpointCount : 0;

        public event Action<int, int> ProgressChanged;
        public event Action<float> Finished;
        public event Action<float> CountdownChanged;
        public event Action RaceStarted;

        private void Start() => ResetRace();

        private void Update()
        {
            if (Phase == RacePhase.Countdown)
            {
                CountdownRemaining = Mathf.Max(0f, CountdownRemaining - Time.deltaTime);
                CountdownChanged?.Invoke(CountdownRemaining);
                if (CountdownRemaining <= 0f)
                {
                    Phase = RacePhase.Racing;
                    vehicle?.SetDrivingEnabled(true);
                    RaceStarted?.Invoke();
                }
                return;
            }

            if (Phase == RacePhase.Racing) ElapsedSeconds += Time.deltaTime;
        }

        public bool TryPassCheckpoint(RaceCheckpoint checkpoint)
        {
            if (Phase != RacePhase.Racing || definition == null || checkpoint == null || checkpoint.Order != NextCheckpoint)
                return false;

            NextCheckpoint++;
            if (NextCheckpoint >= definition.checkpointCount)
            {
                if (CurrentLap >= definition.laps)
                {
                    CompleteRace();
                    return true;
                }
                CurrentLap++;
                NextCheckpoint = 0;
            }

            ProgressChanged?.Invoke(CurrentLap, NextCheckpoint);
            return true;
        }

        public void ResetRace()
        {
            CurrentLap = 1;
            NextCheckpoint = 0;
            ElapsedSeconds = 0f;
            CountdownRemaining = definition != null ? Mathf.Max(0f, definition.countdownSeconds) : 3f;
            Phase = RacePhase.Countdown;
            vehicle?.SetDrivingEnabled(false);
            ProgressChanged?.Invoke(CurrentLap, NextCheckpoint);
        }

        private void CompleteRace()
        {
            if (IsFinished) return;
            Phase = RacePhase.Finished;
            vehicle?.SetDrivingEnabled(false);
            Finished?.Invoke(ElapsedSeconds);
        }
    }
}
