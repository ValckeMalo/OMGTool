namespace OMG.Battle
{
    using OMG.Battle.Data;
    using OMG.Battle.UI;

    using System.Collections;
    using System.Linq;

    using Unity.Behavior;
    using UnityEngine;

    public enum BattleState
    {
        Initialize,
        Player,
        Monsters,
        Won,
        Lose,
    }

    public class BattleSystem : MonoBehaviour
    {
        public delegate void EventNextTurn(BattleState nextState);
        public static EventNextTurn OnNextTurn;

        private static BattleSystem instance = null;

        [SerializeField] private BattleData battleData;
        public static BattleData BattleData { get => instance.battleData; }
        [SerializeField] private RuntimeBlackboardAsset blackboardAsset;

        public Transform[] unitHUDPos;

        private static int turnIndex = 1;
        public static int TurnIndex { get => turnIndex; }

        public void Awake()
        {
            //Singleton
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            //DATA
            battleData = ScriptableObject.Instantiate<BattleData>(battleData);
            battleData.Duplicate();
            blackboardAsset.Blackboard.Variables.Where(x => x.Name == "Player").First().ObjectValue = battleData.PlayerData;

            //HUD
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[0].position, battleData.PlayerData);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[1].position, battleData.MonstersData[0]);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[2].position, battleData.MonstersData[1]);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[3].position, battleData.MonstersData[2]);

            OnNextTurn += NextTurn;
        }

        public void Start() => NextTurn(BattleState.Initialize);

        private void NextTurn(BattleState nextState)
        {
            switch (nextState)
            {
                case BattleState.Initialize:
                    StartCoroutine(InitializeCombat());
                    break;

                case BattleState.Player:
                    PlayerBattleSystem.OnUnitTurn?.Invoke(battleData);
                    break;

                case BattleState.Monsters:
                    turnIndex++;
                    MonstersBattleSystem.OnUnitTurn?.Invoke(battleData);
                    break;

                case BattleState.Won:
                    break;

                case BattleState.Lose:
                    break;

                default:
                    Debug.LogError($"Can't recognize the state send {nextState}.");
                    break;
            }
        }

        private IEnumerator InitializeCombat()
        {
            yield return new WaitUntil(() => InitializeCallBack(PlayerBattleSystem.OnInitialize));
            yield return new WaitUntil(() => InitializeCallBack(MonstersBattleSystem.OnInitialize));

            NextTurn(BattleState.Player);
        }

        private bool InitializeCallBack(UnitBattleSystem.EventInitialize eventInitialize)
        {
            if (eventInitialize == null)
            {
                Debug.LogError($"Can't Initialize {eventInitialize}");
                return false;
            }

            eventInitialize.Invoke(battleData, blackboardAsset.Blackboard);
            return true;
        }
    }
}