namespace OMG.Battle.Data
{
    using OMG.Unit.Monster;
    using OMG.Unit.Player;

    using UnityEngine;

    [CreateAssetMenu(fileName = "NewBattleData", menuName = "Battle/Data", order = 0)]
    public class BattleData : ScriptableObject
    {
        [SerializeField] private Player player;
        [SerializeField] private Monster[] monsters = new Monster[3];

        public Player PlayerData { get => player; }
        public Monster[] MonstersData { get => monsters; }
    }
}