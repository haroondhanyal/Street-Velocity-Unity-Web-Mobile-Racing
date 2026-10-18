using System;
using System.Collections.Generic;
using StreetVelocity.Progression;
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
        private readonly Dictionary<RaceActor, float> rivalProgress = new Dictionary<RaceActor, float>();
        private float raceStartZ;

        public int CurrentLap { get; private set; } = 1;
        public int NextCheckpoint { get; private set; }
        public bool IsFinished => Phase == RacePhase.Finished;
        public float ElapsedSeconds { get; private set; }
        public float CountdownRemaining { get; private set; }
        public RacePhase Phase { get; private set; } = RacePhase.Countdown;
        public RaceDefinition Definition => definition;
        public int CheckpointsPerLap => definition != null ? definition.checkpointCount : 0;
        public int EventIndex { get; private set; }
        public string EventName => new[] { "STREET SPRINT", "CIRCUIT RACE", "DRIFT EVENT", "TIME TRIAL", "POLICE ESCAPE" }[EventIndex];
        public int LapCount => EventIndex == 1 ? 3 : 1;
        public int CreditsReward => new[] { 8000, 12000, 9500, 7500, 14000 }[EventIndex];
        public int ExperienceReward => new[] { 1200, 1800, 1500, 900, 2200 }[EventIndex];
        public bool IsDriftEvent => EventIndex == 2;
        public bool IsTimeTrial => EventIndex == 3;
        public bool IsPoliceEscape => EventIndex == 4;
        public bool WasCaptured { get; private set; }
        public float PolicePressure { get; private set; }
        public float PlayerProgress => vehicle != null ? vehicle.transform.position.z - raceStartZ : 0f;
        public int FinishPosition { get; private set; } = 1;
        public int Position => IsPoliceEscape ? (WasCaptured ? 6 : 1) : IsTimeTrial ? (ElapsedSeconds > 15f ? 2 : 1) : 1 + CountRivalsAhead();
        public float DriftTarget => 3000f;

        public event Action<int, int> ProgressChanged;
        public event Action<float> Finished;
        public event Action<float> CountdownChanged;
        public event Action RaceStarted;

        public void RegisterRival(RaceActor actor)
        {
            if (actor != null && !rivalProgress.ContainsKey(actor)) rivalProgress.Add(actor, actor.transform.position.z - raceStartZ);
        }

        public void ReportRivalProgress(RaceActor actor, float progress)
        {
            if (actor != null && rivalProgress.ContainsKey(actor)) rivalProgress[actor] = progress;
        }

        public void UpdatePolicePressure(float closestDistance, float deltaTime)
        {
            if (Phase != RacePhase.Racing || !IsPoliceEscape) return;
            PolicePressure = closestDistance < 6.2f
                ? Mathf.Min(8f, PolicePressure + deltaTime)
                : Mathf.Max(0f, PolicePressure - deltaTime * 0.65f);
            if (PolicePressure >= 8f)
            {
                WasCaptured = true;
                CompleteRace();
            }
        }

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

            if (Phase == RacePhase.Racing)
            {
                ElapsedSeconds += Time.deltaTime;
                if (IsDriftEvent && vehicle != null && vehicle.DriftScore >= DriftTarget) CompleteRace();
            }
        }

        public bool TryPassCheckpoint(RaceCheckpoint checkpoint)
        {
            if (Phase != RacePhase.Racing || definition == null || checkpoint == null || checkpoint.Order != NextCheckpoint)
                return false;

            if (IsDriftEvent) return true;

            NextCheckpoint++;
            if (NextCheckpoint >= definition.checkpointCount)
            {
                if (CurrentLap >= LapCount)
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
            EventIndex = Mathf.Clamp(PlayerCareerSave.SelectedEvent, 0, 4);
            raceStartZ = vehicle != null ? vehicle.transform.position.z : 0f;
            rivalProgress.Clear();
            PolicePressure = 0f;
            WasCaptured = false;
            FinishPosition = 1;
            CurrentLap = 1;
            NextCheckpoint = 0;
            ElapsedSeconds = 0f;
            CountdownRemaining = definition != null ? Mathf.Max(0f, definition.countdownSeconds) : 3f;
            Phase = RacePhase.Countdown;
            vehicle?.SetDrivingEnabled(false);
            vehicle?.ResetDriftScore();
            ProgressChanged?.Invoke(CurrentLap, NextCheckpoint);
        }

        private void CompleteRace()
        {
            if (IsFinished) return;
            FinishPosition = WasCaptured ? 6 : Position;
            Phase = RacePhase.Finished;
            vehicle?.SetDrivingEnabled(false);
            Finished?.Invoke(ElapsedSeconds);
        }

        private int CountRivalsAhead()
        {
            var ahead = 0;
            foreach (var progress in rivalProgress.Values)
                if (progress > PlayerProgress) ahead++;
            return ahead;
        }
    }
}
