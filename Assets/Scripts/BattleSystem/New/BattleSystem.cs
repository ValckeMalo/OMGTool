namespace OMG.Battle
{
    using MaloProduction.CustomAttributes;

    using OMG.Battle.Data;
    using OMG.Battle.UI;
    using OMG.Battle.Manager;

    using System.Linq;

    using Unity.Behavior;
    using UnityEngine;

    public class BattleSystem : MonoBehaviour
    {
        public enum BattleState
        {
            Oropo,
            Monsters,
            Win,
            Lose,
        }

        #region Singleton
        public static BattleSystem Instance { get; private set; }
        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        #endregion

        [Title("Battle System")]
        [SerializeField] private BattleData battleUnits;
        [SerializeField] private Transform[] unitHUDPos;
        [SerializeField] private RuntimeBlackboardAsset blackboardAsset;
        [SerializeField] private GameBoardManager gameBoard;
        [SerializeField] private MonstersBattleManager monstersBattleManager;

        public GameBoardManager GameBoard { get => gameBoard; }
        public MonstersBattleManager MonstersBattleManager { get => monstersBattleManager; }
        public BattleData BattleData { get => battleUnits; }
        public int TurnIndex { get; private set; } /*I think i need to put it in the gameBoard it's more natural TODO*/

        public void Start() => StartBattle();

        public void StartBattle()
        {
            if (battleUnits == null || unitHUDPos == null || unitHUDPos.Length <= 0 || blackboardAsset == null)
            {
                Debug.LogError($"Some Variable Are Not Set in the BattleSystem\n battleUnits : {battleUnits}\n unitHUDPos : {unitHUDPos}\n blackboardAsset : {blackboardAsset}");
                return;
            }

            TurnIndex = 1;

            //duplicate the data because it's scriptable objects
            battleUnits = Instantiate(battleUnits);
            battleUnits.Duplicate();
            blackboardAsset.Blackboard.Variables.Where(x => x.Name == "Player").First().ObjectValue = battleUnits.GetOropo();

            //HUD
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[0].position, battleUnits.GetOropo());
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[1].position, battleUnits.GetAllMonsters()[0]);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[2].position, battleUnits.GetAllMonsters()[1]);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[3].position, battleUnits.GetAllMonsters()[2]);

            gameBoard = new GameBoardManager(battleUnits.GetOropo().Deck);
            monstersBattleManager = new MonstersBattleManager(battleUnits.GetAllMonsters(), blackboardAsset.Blackboard);

            if (GameBoard == null || MonstersBattleManager == null)
            {
                Debug.LogError($"Some Variable Are Not Set in the BattleSystem\n gameBoard : {GameBoard}\n monstersBattleManager : {MonstersBattleManager}");
                return;
            }

            HUDBattle.EndTurnButton.AddCallback(EndOropoTurn);
            SwitchBattleSate(BattleState.Oropo);
        }

        private void EndOropoTurn()
        {
            GameBoard.EndOropoTurn();
            SwitchBattleSate(BattleState.Monsters);
        }

        private void SwitchBattleSate(BattleState newState)
        {
            switch (newState)
            {
                case BattleState.Oropo:
                    GameBoard.StartOropoTurn();
                    break;

                case BattleState.Monsters:
                    MonstersBattleManager.UpdateMonstersTurn(battleUnits.GetAllMonsters(), battleUnits.GetOropo(), blackboardAsset.Blackboard);
                    SwitchBattleSate(BattleState.Oropo);
                    break;

                case BattleState.Win:
                    break;

                case BattleState.Lose:
                    break;

                default:
                    Debug.LogError($"Can't recognize the newState : {newState} , {(int)newState}");
                    return;
            }
        }
    }
}