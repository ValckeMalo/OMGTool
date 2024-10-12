namespace MaloProduction
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public enum FilterElementType
    {
        Label,
        Enum,
        Int,
        IntSlider,
        Toggle
    }

    public class FilterElement
    {
        public FilterElementType Type { get; private set; }
        public string Label { get; private set; }
        public System.Enum EnumValue { get; set; }
        public int IntValue { get; set; }
        public bool ToggleValue { get; set; }
        public int MinSliderValue { get; private set; }
        public int MaxSliderValue { get; private set; }

        public FilterElement(string label)
        {
            Type = FilterElementType.Label;
            Label = label;
        }

        public FilterElement(System.Enum enumValue)
        {
            Type = FilterElementType.Enum;
            EnumValue = enumValue;
        }

        public FilterElement(int intValue)
        {
            Type = FilterElementType.Int;
            IntValue = intValue;
        }

        public FilterElement(int intValue, int minSliderValue, int maxSliderValue)
        {
            Type = FilterElementType.IntSlider;
            IntValue = intValue;
            MinSliderValue = minSliderValue;
            MaxSliderValue = maxSliderValue;
        }

        public FilterElement(bool toggleValue)
        {
            Type = FilterElementType.Toggle;
            ToggleValue = toggleValue;
        }
    }

    public class Filter
    {
        private bool toggle = false;
        public bool Toggle { get => toggle; }

        private List<FilterElement> filterElements = new List<FilterElement>();
        private int elementsCount = 0;

        public Filter(Filter copy)
        {
            toggle = copy.toggle;
            filterElements = new List<FilterElement>(copy.filterElements);
            elementsCount = copy.elementsCount;
        }

        public Filter(List<FilterElement> filterElements)
        {
            this.filterElements = filterElements;
            elementsCount = filterElements.Count;
        }

        public void Add(FilterElement element)
        {
            filterElements.Add(element);
            elementsCount++;
        }

        public void FilterBox(Rect rect, float spacing, float margin = 5f)
        {
            Vector2 togglePosition = new Vector2(rect.position.x + margin, rect.position.y + margin);
            Vector2 toggleSize = new Vector2(15f, 15f);
            Rect toggleRect = new Rect(togglePosition, toggleSize);

            toggle = EditorGUI.Toggle(toggleRect, toggle);

            //if not toggle disable the line
            GUI.enabled = toggle;

            Vector2 elementPosition = new Vector2(togglePosition.x + toggleSize.x + spacing, togglePosition.y);
            Vector2 elementSize = new Vector2((rect.width - toggleSize.x) / elementsCount, toggleSize.y);
            Rect elementRect = new Rect(elementPosition, elementSize);

            foreach (FilterElement element in filterElements)
            {
                switch (element.Type)
                {
                    case FilterElementType.Label:
                        EditorGUI.LabelField(elementRect, element.Label);
                        break;
                    case FilterElementType.Enum:
                        element.EnumValue = EditorGUI.EnumPopup(elementRect, element.EnumValue);
                        break;
                    case FilterElementType.Int:
                        element.IntValue = EditorGUI.IntField(elementRect, element.IntValue);
                        break;
                    case FilterElementType.IntSlider:
                        element.IntValue = EditorGUI.IntSlider(elementRect, element.IntValue, element.MinSliderValue, element.MaxSliderValue);
                        break;
                    case FilterElementType.Toggle:
                        element.ToggleValue = EditorGUI.Toggle(elementRect, element.ToggleValue);
                        break;
                }

                elementRect.position += new Vector2(elementSize.x, 0f);
            }

            GUI.enabled = true;
        }
    }
}