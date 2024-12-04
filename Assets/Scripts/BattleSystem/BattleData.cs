namespace OMG.Battle.Data
{
    using OMG.Unit.Monster;
    using OMG.Unit.Player;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewBattleData", menuName = "Battle/Data", order = 0)]
    public class BattleData : ScriptableObject
    {
        [SerializeField] private Player oropo;
        [SerializeField] private Monster[] monsters = new Monster[3];

        public void Duplicate()
        {
            oropo = Instantiate(oropo);

            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null)
                {
                    monsters[i] = Instantiate(monsters[i]);
                }
            }
        }

        public Player GetOropo()
        {
            return oropo;
        }
        public Monster GetFirstMonster()
        {
            return monsters[0];
        }
        public Monster GetLastMonster()
        {
            for (int i = monsters.Length - 1; i > 0; i--)
            {
                if (monsters[i] != null)
                {
                    return monsters[i];
                }
            }

            return null;
        }
        public Monster[] GetAllMonsters()
        {
            return monsters.Where(x => x != null).ToArray();
        }
    }
}