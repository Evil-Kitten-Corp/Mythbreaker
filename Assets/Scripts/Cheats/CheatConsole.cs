using System.Collections.Generic;
using System.Linq;
using FinalScripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class CheatConsole : MonoBehaviour
    {
        public KeyCode key;
        private bool _showConsole;

        string _input;

        public static DebugCheat KILL_ALL;
        public static DebugCheat INVUL;
        public static DebugCheat ALL_POWERS;
        public static DebugCheat UNLOCK_BOSS;
        public static DebugCheat REGINALD;

        private List<object> _commandList;

        private void Awake()
        {
            KILL_ALL = new DebugCheat("kill_all", "kill_all", () =>
            {
                List<EnemyBehaviour> allEnemies = FindObjectsOfType<EnemyBehaviour>().ToList();

                foreach (var enemy in allEnemies)
                {
                    enemy.OnDeath?.Invoke(enemy.gameObject);
                }
            });

            INVUL = new DebugCheat("inv", "invul", () =>
            {
                AttributesManager am = FindObjectOfType<AttributesManager>();

                am.SetInvulnerable(!am.IsInvulnerable());
            });

            ALL_POWERS = new DebugCheat("powers_all", "p_all", () =>
            {
                WaveFinal rew = FindObjectOfType<WaveFinal>();

                foreach (var ability in rew.rewards.Keys)
                {
                    ability.Unlock?.Invoke();
                }
            });
            
            UNLOCK_BOSS = new DebugCheat("u_boss", "unlock_boss", () =>
            {
                GameObject ob = GameObject.FindWithTag("UnlockBoss");
                ob.SetActive(false);
            });
            
            REGINALD = new DebugCheat("reggie", "reginald", () => {});

            _commandList = new List<object>
            {
                KILL_ALL,
                INVUL,
                ALL_POWERS,
                UNLOCK_BOSS,
                REGINALD
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                _showConsole = !_showConsole;

                if (_showConsole)
                {
                    MythUIElement.IsInUI = true;
                    Time.timeScale = 0;
                }
                else
                {
                    MythUIElement.IsInUI = false;
                    Time.timeScale = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && _showConsole)
            {
                HandleInput();
                _input = "";
            }
        }

        private void OnGUI()
        {
            if (!_showConsole) return;

            float y = 0f;
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            _input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), _input);
        }

        private void HandleInput()
        {
            for (int i = 0; i < _commandList.Count; i++)
            {
                if (_commandList[i] is DebugCheat cheatBase && _input.Contains(cheatBase.cheatId))
                {
                    cheatBase.Invoke();
                }
            }
        }
    }
}