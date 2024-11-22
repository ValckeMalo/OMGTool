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

        public void Duplicate()
        {
            player = Instantiate(player);

            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null)
                {
                    monsters[i] = Instantiate(monsters[i]);
                }
            }
        }
    }
}