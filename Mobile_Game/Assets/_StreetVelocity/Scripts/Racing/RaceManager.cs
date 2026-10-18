using System;
using UnityEngine;

namespace StreetVelocity.Racing
{
    public sealed class RaceManager : MonoBehaviour
    {
        [SerializeField] private RaceDefinition definition;
        [SerializeField] private Transform player;
        public int CurrentLap { get; private set; } = 1;
        public int NextCheckpoint { get; private set; }
        public bool IsFinished { get; private set; }
        public float ElapsedSeconds { get; private set; }
        public event Action<int, int> ProgressChanged;
        public event Action<float> Finished;

        private void Update()
        {
            if (!IsFinished && definition != null && player != null) ElapsedSeconds += Time.deltaTime;
        }

        public bool TryPassCheckpoint(RaceCheckpoint checkpoint)
        {
            if (IsFinished || definition == null || checkpoint == null || checkpoint.Order != NextCheckpoint) return false;
            NextCheckpoint++;
            if (NextCheckpoint >= definition.checkpointCount)
            {
                NextCheckpoint = 0;
                if (++CurrentLap > definition.laps)
                {
                    IsFinished = true;
                    Finished?.Invoke(ElapsedSeconds);
                }
            }
            ProgressChanged?.Invoke(CurrentLap, NextCheckpoint);
            return true;
        }

        public void ResetRace()
        {
            CurrentLap = 1; NextCheckpoint = 0; ElapsedSeconds = 0f; IsFinished = false;
        }
    }
}
