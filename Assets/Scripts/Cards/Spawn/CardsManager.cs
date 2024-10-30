using MaloProduction.CustomAttributes;
using UnityEngine;
using System.Collections.Generic;

public class CardsManager : MonoBehaviour
{
    public delegate void EventCardSpawn(CardData cardToSpawn);
    public static EventCardSpawn onCardSpawn;

    public CardData[] cardsToSpawn;

    [Button("Spawn Cards array")]
    private void OnCallDelegate()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < cardsToSpawn.Length; i++)
            {
                onCardSpawn?.Invoke(cardsToSpawn[i]);
            }
        }
        else
        {
            Debug.LogError("Youre application is not playing");
        }
    }
}