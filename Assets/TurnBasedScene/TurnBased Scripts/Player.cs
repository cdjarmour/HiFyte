using UnityEngine;
using System;
using System.Collections.Generic;

namespace Battle {
    public class Player : MonoBehaviour {
        private List<StatusEffect> playerStates = new List<StatusEffect>();
        private List<int> stateDuration = new List<int>();

        private AttackAction[] moves;

        private int HP;

        private int decisionBeatWindow;
        private int rhythmBeatWindow;


        public static event Action<StatusEffect> PlayerStateChanged;

        public void UpdateStatusEffect(StatusEffect statusEffect) {
            playerStates.Add(statusEffect);

            PlayerStateChanged?.Invoke(statusEffect);
        }

        public void setAttacks(AttackAction[] attacks) {
            moves = attacks;
        }

        public AttackAction[] getMoveList() {
            return moves;
        }

        public AttackAction getAction(int index) {
            return moves[index];
        }

        public void setHP(int hp) {
            HP = hp; 
        }
    }

}