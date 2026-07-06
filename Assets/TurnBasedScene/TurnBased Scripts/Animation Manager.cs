using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator enemyAnimator;

    void OnEnable() {
        BattleManager.OnBattleStateChange += OnPlayerStateAnimation;
    }

    void OnDisable() {
        BattleManager.OnBattleStateChange -= OnPlayerStateAnimation;
    }

    private void OnPlayerStateAnimation(BattleState state) {
        switch (state) {
            case BattleState.EndTurn:
            PlayAttack();
            break;
            default:
            PlayIdle();
            break;
        }
    }

    private void PlayAttack() {
        float beatLength = BeatManager.BeatLength(BattleData.instance.getBPM());
        float desiredDuration = beatLength * 4f; // 4 beats worth of time

        playerAnimator.Play("Attack", 0, 0f);
        AnimatorStateInfo info = playerAnimator.GetCurrentAnimatorStateInfo(0);
        float clipLength = info.length;

        playerAnimator.speed = clipLength / desiredDuration;
    }

    private void PlayIdle() {
        playerAnimator.speed = 1f;
        playerAnimator.Play("Idle");
    }
}