using System.Collections.Generic;
using System.Linq;
using FinalScripts;
using FinalScripts.Refactored;
using UnityEngine;

namespace DefaultNamespace
{
    public class CheatConsole : MonoBehaviour
    {
        public KeyCode key;
        private bool _showConsole;
        bool userHasHitReturn = false;

        string _input;

        public static DebugCheat KILL_ALL;
        public static DebugCheat INVUL;
        public static DebugCheat ALL_POWERS;
        public static DebugCheat UNLOCK_BOSS;
        public static DebugCheat REGINALD;
        public static DebugCheat OP;
        public static DebugCheat ANTI_CC;
        public static DebugCheat STATS;

        private List<object> _commandList;
        
        private AttributesManager _am;
        private EnemyBT[] _enemyBehaviours;
        private WaveFinal _rew;
        private TeleportIndicator _ob;

        private void Awake()
        {
            _ob = FindObjectOfType<TeleportIndicator>(true);
            _rew = FindObjectOfType<WaveFinal>();
            _enemyBehaviours = FindObjectsOfType<EnemyBT>();
            _am = FindObjectOfType<AttributesManager>();
            
            STATS = new DebugCheat("stats", "stats", () =>
            {
                Debug.Log($"Life: {_am.health.Amount}. Base Damage: {_am.Attack}. Can be knocked: {_am.CanStomp()}. " +
                          $"Can be damaged: {!_am.IsInvulnerable()}");
            });
            
            ANTI_CC = new DebugCheat("no_cc", "no_cc", () =>
            {
                _am.SetKnockUp(false);
            });
            
            OP = new DebugCheat("op", "op", () =>
            {
                _am.Attack = 1000;
            });

            KILL_ALL = new DebugCheat("kill_all", "kill_all", () =>
            {
                List<EnemyBT> allEnemies = _enemyBehaviours.ToList();

                foreach (var enemy in allEnemies)
                {
                    enemy.TriggerDeath();
                }
            });

            INVUL = new DebugCheat("inv", "invul", () =>
            {
                _am.SetInvulnerable(!_am.IsInvulnerable());
            });

            ALL_POWERS = new DebugCheat("powers_all", "p_all", () =>
            {
                foreach (var ability in _rew.rewards.Keys)
                {
                    ability.Unlock?.Invoke();
                }
            });
            
            UNLOCK_BOSS = new DebugCheat("u_boss", "unlock_boss", () =>
            {
                _ob.gameObject.SetActive(true);
            });
            
            REGINALD = new DebugCheat("reggie", "reginald", () => {});

            _commandList = new List<object>
            {
                KILL_ALL,
                INVUL,
                ALL_POWERS,
                UNLOCK_BOSS,
                REGINALD,
                OP,
                ANTI_CC,
                STATS
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

            if (Input.GetKeyDown(KeyCode.Return) && _showConsole || userHasHitReturn)
            {
                HandleInput();
                _input = "";
                userHasHitReturn = false;
            }
        }

        private void OnGUI()
        {
            if (!_showConsole) return;
            
            Event e = Event.current;
            
            if (e.keyCode == KeyCode.Return)
            {
                userHasHitReturn = true;
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