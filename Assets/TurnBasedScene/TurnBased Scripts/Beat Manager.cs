using UnityEngine;
using System;
using System.Collections;

namespace Battle {
    public class BeatManager : MonoBehaviour {

        private const int COUNTDOWN_TIME = 8;

        private const int PLAYER_DECISION_TIME = 16;
        private const int PLAYER_PLAY_TIME = 16;


        public static BeatManager instance { get; private set; }
        AudioSource audioSource;
        private float preciseTime;
        private float beatLength;
        private bool timer;

        private float loopPos;

        private float playerDecisionWindow;
        private int countDown = 4;

        public static event Action OnTimerExpired;
        public static event Action<int> OnCountdown;
        public static event Action OnCountDownEnd;
        public static event Action OnSongLoop;

        private float startPos;
        private float endPos;

        private void Awake() {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            beatLength = getBeatLength();
            loopPos = beatLength * 16;
        }

        private void OnEnable() {
            BattleManager.OnBattleStateChange += OnStateCountdown;
        }

        private int lastLoggedBeat = -1;

        void Update() {
            preciseTime = (float) ( (double) audioSource.timeSamples / audioSource.clip.frequency);

            if (audioSource.timeSamples >= audioSource.clip.samples) {
                audioSource.timeSamples = Mathf.FloorToInt(loopPos * audioSource.clip.frequency);
                OnSongLoop?.Invoke();
            }


            if (!timer) return;

            int beatsSinceStart = Mathf.FloorToInt((preciseTime - startPos) / beatLength);
            if (beatsSinceStart != lastLoggedBeat && beatsSinceStart >= 0) {
                Debug.Log($"Beat {beatsSinceStart}");
                lastLoggedBeat = beatsSinceStart;
            }

            if (preciseTime - startPos >= beatLength * (playerDecisionWindow + (10f / BattleData.instance.getSpeed()))) {
                timer = false;
                OnTimerExpired?.Invoke();
            }
        }



        private void OnStateCountdown(BattleState state) {
            timer = false;
            switch (state) {
                case BattleState.PlayerTurn:
                StartTimer(PLAYER_DECISION_TIME);
                break;
                case BattleState.PlayerAttack:
                StartDelayedTimer(PLAYER_PLAY_TIME);
                break;
                case BattleState.EndTurn:
                StartTimer(4);
                break;
                case BattleState.EnemyTurn:
                StartTimer(16);
                break;
            }


        }





        public void StartTimer(int beats) {
            startPos = Mathf.Ceil(preciseTime / beatLength) * beatLength;
            playerDecisionWindow = beats - (10f / BattleData.instance.getSpeed());
            timer = true;
            Debug.Log(beatLength);
        }


        public void StartDelayedTimer(int beats) {
            StartCoroutine(DelayedStart(beats));
        }


        private IEnumerator DelayedStart(int beats) {
            float delayStart = Mathf.Ceil(preciseTime / beatLength) * beatLength;

            for (int i = 0; i <= COUNTDOWN_TIME; i++) {
                OnCountdown?.Invoke(COUNTDOWN_TIME - i);
                float nextBeatTime = delayStart + beatLength * i;
                while (preciseTime < nextBeatTime) {
                    yield return null;
                }
            }

            startPos = delayStart + beatLength * COUNTDOWN_TIME;
            playerDecisionWindow = beats;
            timer = true;
            endPos = startPos + beatLength * beats;
            OnCountDownEnd?.Invoke();
        }

        public float getEndPos() {
            return endPos + beatLength * (10f / BattleData.instance.getSpeed());
        }

        public float getStartPos() {
            return startPos + beatLength * (10f / BattleData.instance.getSpeed());
        }

        public float getBeatLength() {
            return 60f / BattleData.instance.getBPM();
        }

        public float getTime() {
            return preciseTime;
        }
    }
}
