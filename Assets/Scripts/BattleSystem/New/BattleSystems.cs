namespace OMG.Battle
{
    using MaloProduction.CustomAttributes;

    using OMG.Battle.Data;
    using OMG.Battle.UI;
    using OMG.Battle.Manager;

    using System.Linq;

    using Unity.Behavior;
    using UnityEngine;

    public class BattleSystems : MonoBehaviour
    {
        [Title("Battle System")]
        [SerializeField] private BattleData battleUnits;
        [SerializeField] private Transform[] unitHUDPos;
        [SerializeField] private RuntimeBlackboardAsset blackboardAsset;

        private static GameBoardManager gameBoard;
        public static int TurnIndex { get; private set; }

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
            blackboardAsset.Blackboard.Variables.Where(x => x.Name == "Player").First().ObjectValue = battleUnits.OropoData;

            //HUD
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[0].position, battleUnits.OropoData);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[1].position, battleUnits.MonstersData[0]);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[2].position, battleUnits.MonstersData[1]);
            HUDBattle.UnitHUD.SpawnUnitHUD(unitHUDPos[3].position, battleUnits.MonstersData[2]);

            gameBoard = new GameBoardManager(battleUnits.OropoData.Deck);

            HUDBattle.EndTurnButton.AddCallback(EndOropoTurn);
        }

        private void EndOropoTurn()
        {
            gameBoard.EndOropoTurn();
            SwitchBattleSate(BattleState.Monsters);
        }

        private void SwitchBattleSate(BattleState newState)
        {
            switch (newState)
            {
                case BattleState.Player:
                    gameBoard.StartOropoTurn();
                    break;

                case BattleState.Monsters:
                    break;

                case BattleState.Won:
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