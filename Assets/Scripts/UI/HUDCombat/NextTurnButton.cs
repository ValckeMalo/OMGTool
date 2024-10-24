using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NextTurnButton : MonoBehaviour
{
    private Slider sliderTime;
    private Button nextTurnButton;

    [SerializeField] private float maxSecondPlayerTurn = 60;

    void Start()
    {
        sliderTime = GetComponent<Slider>();
        sliderTime.minValue = 0;
        sliderTime.maxValue = maxSecondPlayerTurn;
        sliderTime.value = sliderTime.maxValue;

        nextTurnButton = GetComponentInChildren<Button>();
        nextTurnButton.onClick.AddListener(NextTurnClicked);

        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (sliderTime.value >= sliderTime.minValue)
        {
            sliderTime.value -= Time.deltaTime;
            yield return null;
        }

        NextTurnClicked();
    }

    private void NextTurnClicked()
    {
        BattleSystem.OnNextTurn?.Invoke();
        StopAllCoroutines();
        sliderTime.value = sliderTime.maxValue;
        nextTurnButton.interactable = false;
    }
}