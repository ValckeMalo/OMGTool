namespace OMG.Battle
{
    using MVProduction.CustomAttributes;
    using OMG.Battle.Data;
    using OMG.Battle.Manager;
    using OMG.Battle.UI;
    using OMG.Battle.UI.Manager;
    using OMG.Unit.Monster;
    using System.Collections;
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
        [SerializeField] private GameBoardManager gameBoard;
        [SerializeField] private MonstersBattleManager monstersBattleManager;

        public GameBoardManager GameBoard { get => gameBoard; } //TODO MEH
        public MonstersBattleManager MonstersBattleManager { get => monstersBattleManager; }//TODO MEH
        public BattleData BattleData { get => battleUnits; }
        public int TurnIndex { get; private set; } /*I think i need to put it in the gameBoard it's more natural TODO*/
        private int remainMonsterAlive => battleUnits.GetAllMonsters().Length;//TODO REDO ALL THE DEATH LOGIC

        public void Start() => StartBattle();

        public void StartBattle()
        {
            if (battleUnits == null || unitHUDPos == null || unitHUDPos.Length <= 0)
            {
                Debug.LogError($"Some Variable Are Not Set in the BattleSystem\n battleUnits : {battleUnits}\n unitHUDPos : {unitHUDPos}\n ");
                return;
            }

            TurnIndex = 1;

            //duplicate the data because it's scriptable objects
            battleUnits = Instantiate(battleUnits);
            battleUnits.Duplicate();

            //HUD
            UnitHUDSpawner.OnSpawnUnitHUD.Invoke(unitHUDPos[0].position, battleUnits.GetOropo(), false);
            UnitHUDSpawner.OnSpawnUnitHUD.Invoke(unitHUDPos[1].position, battleUnits.GetAllMonsters()[0], true);
            UnitHUDSpawner.OnSpawnUnitHUD.Invoke(unitHUDPos[2].position, battleUnits.GetAllMonsters()[1], true);
            UnitHUDSpawner.OnSpawnUnitHUD.Invoke(unitHUDPos[3].position, battleUnits.GetAllMonsters()[2], true);

            gameBoard = new GameBoardManager(battleUnits.GetOropo().Deck);
            monstersBattleManager = new MonstersBattleManager(battleUnits.GetAllMonsters());

            if (GameBoard == null || MonstersBattleManager == null)
            {
                Debug.LogError($"Some Variable Are Not Set in the BattleSystem\n gameBoard : {GameBoard}\n monstersBattleManager : {MonstersBattleManager}");
                return;
            }

            HUDBattle.Instance.TurnButtonAddCallback(EndOropoTurn);//TODO MEH
            SwitchBattleSate(BattleState.Oropo);
        }

        public void EndOropoTurn()//TODO EVENT ??
        {
            GameBoard.EndOropoTurn();
            SwitchBattleSate(BattleState.Monsters);
        }

        public void OropoDeath()//TODO EVENT ??
        {
            SwitchBattleSate(BattleState.Lose);
        }

        public void MobDead(Monster mobDead)
        {
            battleUnits.DeadMob(mobDead);

            if (remainMonsterAlive <= 0)
            {
                SwitchBattleSate(BattleState.Win);
            }
        }

        private void SwitchBattleSate(BattleState newState)
        {
            switch (newState)
            {
                case BattleState.Oropo:
                    StartCoroutine(OropoTurn());
                    break;

                case BattleState.Monsters:
                    StartCoroutine(MonsterTurn());
                    break;

                case BattleState.Win:
                    Debug.LogError("WIN");
                    Application.Quit();
                    break;

                case BattleState.Lose:
                    Debug.LogError("LOSE");
                    Application.Quit();
                    break;

                default:
                    Debug.LogError($"Can't recognize the newState : {newState} , {(int)newState}");
                    return;
            }
        }

        private IEnumerator MonsterTurn()
        {
            HUDBattle.Instance.TweenTest(true);
            HUDBattle.Instance.SwitchState(HUDBattle.BattleHUDState.Monsters);
            yield return new WaitForSeconds(2f);

            MonstersBattleManager.UpdateMonstersTurn(battleUnits.GetAllMonsters(), battleUnits.GetOropo());
            SwitchBattleSate(BattleState.Oropo);
        }
        private IEnumerator OropoTurn()
        {
            HUDBattle.Instance.TweenTest(false);
            yield return new WaitForSeconds(2f);
            HUDBattle.Instance.SwitchState(HUDBattle.BattleHUDState.Oropo);

            battleUnits.GetOropo().UpdateUnit();
            GameBoard.StartOropoTurn();
        }
    }
}