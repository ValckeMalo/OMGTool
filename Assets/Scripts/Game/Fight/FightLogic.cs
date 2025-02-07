namespace OMG.Game.Fight
{
    using OMG.Game.Fight.Entities;
    using OMG.Unit.Status;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class FightLogic
    {
        public static FightLogic instance;//TODO CHANGE THAT QUICKLY TEMP FOR EASLY DEBUG

        private FightCharacterEntity fightCharacter;
        private List<FightMobEntity> fightMobEntities;
        private int nbTurn = 0;

        public FightLogic(FightCharacterEntity character, List<FightMobEntity> fightMobEntities)
        {
            fightCharacter = character;
            this.fightMobEntities = fightMobEntities;
        }

        public int NbTurn => nbTurn;

        public int MobCount => fightMobEntities.Count;
        public FightCharacterEntity FightCharacterEntity { get => fightCharacter; }
        public FightMobEntity FirstFightMobEntity { get => fightMobEntities.First(); }
        public FightMobEntity LastFightMobEntity { get => fightMobEntities.Last(); }
        public List<FightMobEntity> FightMobEntities { get => fightMobEntities; }
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
    }
}