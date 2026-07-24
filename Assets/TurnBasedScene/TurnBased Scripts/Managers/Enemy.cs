using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Battle {
    public class Enemy : MonoBehaviour {
        [SerializeField] private List<AttackAction> attack = new List<AttackAction>();

        private int health;

        public static event Action<int> OnHealthChange;

        private void OnEnable() {
            health = 100;
            ActionManager.OnPlayerAttack += OnPlayerAction;
        }

        private void Start() {
            OnHealthChange?.Invoke(health);
        }



        private void OnPlayerAction(AttackAction action) {
            doDamage(action.damage);
        }



        private void doDamage(int damage) {
            health -= damage;
            OnHealthChange?.Invoke(health);
        }

        public int getHealth() {
            return health;
        }
    }
}