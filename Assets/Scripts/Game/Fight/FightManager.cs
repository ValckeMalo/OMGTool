namespace OMG.Game.Fight
{
    using MVProduction.CustomAttributes;

    using OMG.Game.Fight.Entities;

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FightManager : MonoBehaviour
    {
        #region Singelton
        private static FightManager instance;
        public static FightManager Instance => instance;

        public void Awake()
        {
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }
        #endregion

        [Title("Fight Manager")]
        [SerializeField, ReadOnly] private FightData fightData;
        private bool mobsSpawned = false;

        public FightData FightData => fightData;

        private void Start()
        {
            StartCoroutine(InitializeFight());
        }

        private IEnumerator InitializeFight()
        {
            fightData.InitializeData(new FightCharacterEntity(),new List<FightMobEntity>());
            StartCoroutine(SpawnMobs());
            PrepareMobs();

            yield return new WaitUntil(() => mobsSpawned);

            PrepareCharacter();
        }

        //TODO: Possibl a StateMachine with each state for one coroutine
        //And we can stock the coroutine in a variable to stop it and start another one
        //Coroutine fightStateCoroutine;
        //A méditer

        private IEnumerator SpawnMobs()
        {
            mobsSpawned = true;
            yield return null;
            //spawn the ui of the entity
        }
        private void PrepareMobs()
        {
            foreach (FightMobEntity mob in fightData.AllMobs)
            {
                mob.InitializeMob(250,250,null,null);
            }
        }
        private void PrepareCharacter()
        {
            //spawn card etc...
        }
        private IEnumerator NewMobsTurn()
        {
            foreach (FightMobEntity mob in fightData.AllMobs)
            {
                yield return null;// new WaitUntil(() => mob.NewTurn());
            }
        }
        private void EndMobsTurn()
        {
            foreach (FightMobEntity mob in fightData.AllMobs)
            {
                mob.EndTurn();
            }
        }
        private IEnumerator NewCharacterTurn()
        {
            yield return null;
        }
    }
}