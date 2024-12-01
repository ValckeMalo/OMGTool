namespace OMG.Battle.Data
{
    using OMG.Unit;
    using OMG.Unit.Monster;
    using OMG.Unit.Player;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewBattleData", menuName = "Battle/Data", order = 0)]
    public class BattleData : ScriptableObject
    {
        [SerializeField] private Player oropo;
        [SerializeField] private Monster[] monsters = new Monster[3];

        public Player OropoData { get => oropo; }
        public Monster[] MonstersData { get => monsters; }

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

        public IUnit GetOropo()
        {
            return oropo as IUnit;
        }
        public IUnit GetFirstMonster()
        {
            return monsters[0] as IUnit;
        }
        public IUnit GetLastMonster()
        {
            for (int i = monsters.Length - 1; i > 0; i--)
            {
                if (monsters[i] != null)
                {
                    return monsters[i] as IUnit;
                }
            }

            return null;
        }
        public IUnit[] GetAllMonsters()
        {
            return monsters.Where(x => x != null).ToArray();
        }
    }
}