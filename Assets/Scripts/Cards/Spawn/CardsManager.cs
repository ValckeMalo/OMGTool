using MaloProduction.CustomAttributes;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private CardOptions cardOptions;
    [SerializeField] private GameObject prefabCard;

    public delegate void EventCardSpawn();
    public static EventCardSpawn onCardSpawn;

    [Button("OnCallDelegate")]
    private void OnCallDelegate()
    {
        if (Application.isPlaying)
        {
            onCardSpawn?.Invoke();
        }
    }
}