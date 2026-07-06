using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class Enemy : MonoBehaviour {
        private int health = 100;



        public void doDamage(int damage) {
            health =- damage;
        }

        public int getHealth() {
            return health;
        }
    }
}