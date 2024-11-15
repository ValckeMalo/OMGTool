namespace OMG.Unit.HUD
{
    using MaloProduction.CustomAttributes;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitHUD : MonoBehaviour
    {
        #region Class
        [System.Serializable]
        private class LifeSlider
        {
            private enum SliderState
            {
                Life,
                Armor,
            }

            [Title("Life Slider")]
            [SerializeField] private Color lifeColor;
            [SerializeField] private Color lifeBackgroundColor;

            [SerializeField] private Color armorColor;
            [SerializeField] private Color armorBackgroundColor;

            [SerializeField] private Slider lifeSlider;
            [SerializeField] private Image lifeBackground;
            [SerializeField] private Image lifeFill;
            [SerializeField] private Slider hit;

            [SerializeField] private Image armorImage;
            [SerializeField] private TextMeshProUGUI armorText;
            [SerializeField] private TextMeshProUGUI lifeText;

            private SliderState cState = SliderState.Life;

            public void Initialize(UnitData data)
            {
                lifeSlider.maxValue = data.maxHp;

                UpdateSlider(data);
            }

            public void UpdateSlider(UnitData data)
            {
                if (data.armor > 0)
                {
                    if (cState != SliderState.Armor)
                    {
                        SetStateArmor();
                    }

                    UpdateArmorText(data.armor);
                }
                else if (cState != SliderState.Life)
                {
                    SetStateLife();
                }

                UpdateLifeSlider(data.hp, data.maxHp);
            }

            private void SetStateArmor()
            {
                lifeFill.color = armorColor;
                lifeBackground.color = armorBackgroundColor;

                armorImage.gameObject.SetActive(true);

                cState = SliderState.Armor;
            }

            private void SetStateLife()
            {
                lifeBackground.color = lifeBackgroundColor;
                lifeFill.color = lifeColor;

                armorImage.gameObject.SetActive(false);

                cState = SliderState.Life;
            }

            private void UpdateArmorText(int armor)
            {
                armorText.text = armor.ToString();
            }

            private void UpdateLifeSlider(int hp, int maxHp)
            {
                if (hp != (int)lifeSlider.value)
                {
                    lifeSlider.value = hp;
                    lifeText.text = $"{hp} / {maxHp}";

                    //TODO Make Tween with the hit slider
                }
            }
        }
        #endregion

        [SerializeField] private UnitData unitData;
        [SerializeField] private LifeSlider lifeSlider;

        private void Start() => lifeSlider.Initialize(unitData);

        private void Update() => lifeSlider.UpdateSlider(unitData);
    }
}