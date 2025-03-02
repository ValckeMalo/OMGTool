namespace OMG.Game.Fight
{
    using MVProduction.CustomAttributes;
    using OMG.Game.Fight.Entities;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    [System.Serializable]
    public class FightData
    {
        [Title("Fight Data")]
        [SerializeField] private FightCharacterEntity fightCharacter;
        [SerializeField] private List<FightMobEntity> fightMobEntities;
        [SerializeField, ReadOnly] private int turnCount = 0;

        public int TurnCount => turnCount;
        public void NewTurn() => turnCount++;

        public int MobCount => fightMobEntities.Count;
        public FightCharacterEntity FightCharacterEntity { get => fightCharacter; }
        public FightMobEntity FirstFightMobEntity { get => fightMobEntities.First(); }
        public FightMobEntity LastFightMobEntity { get => fightMobEntities.Last(); }
        public List<FightMobEntity> AllMobs { get => fightMobEntities; }
        public FightMobEntity WeakestFightMobEntity()
        {
            FightMobEntity weakestMob = null;

            foreach (FightMobEntity currentMobEntity in fightMobEntities)
            {
                if (weakestMob == null || weakestMob.EntityCurrentHealth > currentMobEntity.EntityCurrentHealth)
                {
                    weakestMob = currentMobEntity;
                }
            }

            return weakestMob;
        }
        public FightMobEntity GetRandomMob(int posExecption = -1)
        {
            int indexChoose = Random.Range(0, fightMobEntities.Count);
            if (indexChoose == posExecption)
            {
                indexChoose++;
                indexChoose %= fightMobEntities.Count;
            }

            return fightMobEntities[indexChoose];
        }
        public FightMobEntity GetRandomMobWithStatus(StatusType status)
        {
            List<FightMobEntity> mobWithStatus = fightMobEntities.Where(x => x.HasStatus(status)).Select(x => x).ToList();

            if (mobWithStatus == null)
            {
                Debug.Log($"No Mob found with this status {status}");
                return null;
            }

            return mobWithStatus[Random.Range(0, mobWithStatus.Count)];
        }
        public FightEntity GetRandomEntity()
        {
            List<FightEntity> allEntities = new List<FightEntity>
            {
                fightCharacter
            };
            allEntities.AddRange(fightMobEntities);

            return allEntities[Random.Range(0, allEntities.Count)];

        }


        public void InitializeData(FightCharacterEntity fightCharacter, List<FightMobEntity> fightMobEntities)
        {
            this.fightCharacter = fightCharacter;
            this.fightMobEntities = fightMobEntities;
            turnCount = 1;
        }
    }
}