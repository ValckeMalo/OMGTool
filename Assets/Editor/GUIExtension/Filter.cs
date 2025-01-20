namespace MVProduction
{
    using System.Collections.Generic;
    using System.Drawing.Printing;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class FilterStringFormatter
    {
        private static string[] filterDescriptions = new string[2] { "Filter Selected", "Filters Selected" };
        private static string[] filterCountDescriptions = new string[5] { "No ", "One ", "Two ", "Three ", "Four " };

        /// <summary>
        /// Returns a formatted string that describes the number of filters selected.
        /// </summary>
        /// <param name="count">The number of filters selected (from 0 to 4).</param>
        /// <returns>A formatted string describing the number of filters selected.</returns>
        public static string GetFilterDescription(int count)
        {
            if (count == 1)
            {
                return filterCountDescriptions[count] + filterDescriptions[0];
            }

            return filterCountDescriptions[count] + filterDescriptions[1];
        }
    }

    public enum FilterElementType
    {
        Label,
        Enum,
        Int,
        IntSlider,
        Toggle
    }

    public enum LabelSizeMode
    {
        FitToLabel,
        StretchToFit,
    }

    public class FilterElement
    {
        public FilterElementType Type { get; private set; }
        public LabelSizeMode Mode { get; private set; }
        public string Label { get; private set; }
        public System.Enum EnumValue { get; set; }
        public int IntValue { get; set; }
        public bool ToggleValue { get; set; }
        public int MinSliderValue { get; private set; }
        public int MaxSliderValue { get; private set; }

        public FilterElement(string label, LabelSizeMode mode = LabelSizeMode.FitToLabel)
        {
            Type = FilterElementType.Label;
            Label = label;
            Mode = mode;
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

    public class FilterLine
    {
        private bool toggle = false;
        public bool Toggle { get => toggle; }

        private List<FilterElement> filterElements = new List<FilterElement>();
        private int elementsCount = 0;
        public int ElementsCount { get; }
        public int LastIndex { get => elementsCount - 1; }

        public FilterLine(FilterLine copy)
        {
            toggle = copy.toggle;
            filterElements = new List<FilterElement>(copy.filterElements);
            elementsCount = copy.elementsCount;
        }

        public FilterLine(List<FilterElement> filterElements)
        {
            this.filterElements = filterElements;
            elementsCount = filterElements.Count;
        }

        public void Add(FilterElement element)
        {
            filterElements.Add(element);
            elementsCount++;
        }

        public FilterElementType GetTypeElementAt(int index)
        {
            return filterElements[index].Type;
        }

        public FilterElement GetElementAt(int index)
        {
            return filterElements[index];
        }

        public void DrawFilterLine(Rect rect, float spacing, float margin = 5f)
        {
            Vector2 togglePosition = new Vector2(rect.position.x + margin, rect.position.y + margin);
            Vector2 toggleSize = new Vector2(15f, 15f);
            Rect toggleRect = new Rect(togglePosition, toggleSize);

            toggle = EditorGUI.Toggle(toggleRect, toggle);

            //if not bool is not toggle disable the line
            GUI.enabled = toggle;

            //get the start pose of all elements after the toggle and spacing
            Vector2 elementPosition = new Vector2(togglePosition.x + toggleSize.x + spacing, togglePosition.y);
            //get all size elements can fit in minus (margin * 2f), because toggle add the margin once, to not touch the border
            float maxElementsSizeX = rect.width - toggleSize.x - margin * 2f;
            //get the size for one element minus all the spacing between elements
            Vector2 elementSize = new Vector2((maxElementsSizeX - (spacing * elementsCount)) / elementsCount, toggleSize.y);
            //final rect for one Element
            Rect elementRect = new Rect(elementPosition, elementSize);

            int elementRemainCount = elementsCount;
            bool elementSizeChange = false;
            float maxElementsSizeXRemain = maxElementsSizeX;
            foreach (FilterElement element in filterElements)
            {
                switch (element.Type)
                {
                    case FilterElementType.Label:

                        switch (element.Mode)
                        {
                            case LabelSizeMode.FitToLabel:
                                float widthLabel = GUI.skin.label.CalcSize(new GUIContent(element.Label)).x;
                                elementRect.width = widthLabel;
                                EditorGUI.LabelField(elementRect, element.Label);

                                elementRemainCount--;
                                maxElementsSizeXRemain -= widthLabel;
                                elementSize.x = (maxElementsSizeXRemain - (spacing * elementRemainCount)) / elementRemainCount;
                                elementRect.width = elementSize.x;
                                elementRect.x += widthLabel;
                                elementSizeChange = true;
                                break;

                            case LabelSizeMode.StretchToFit:
                                EditorGUI.LabelField(elementRect, element.Label);
                                break;

                            default:
                                break;
                        }

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

                elementRemainCount--;
                if (!elementSizeChange)
                {
                    maxElementsSizeXRemain -= elementSize.x;
                    elementRect.x += elementSize.x + spacing;
                }
                elementSizeChange = false;
            }

            GUI.enabled = true;
        }
    }

    public class Filter
    {
        private List<FilterLine> filterLines;

        public Filter(List<FilterLine> filterLines)
        {
            this.filterLines = filterLines;
        }

        public void FilterBox(Rect rect, Texture2D background)
        {
            using (new GUI.GroupScope(rect, EditorStyles.helpBox))
            {
                Rect filterLineRect = new Rect(Vector2.zero, new Vector2(rect.width, 20f));
                foreach (FilterLine filterLine in filterLines)
                {
                    filterLine.DrawFilterLine(filterLineRect, 5f);
                    filterLineRect.position += new Vector2(0f, 20f);
                }
            }
        }

        public int ToggleCount(bool state)
        {
            return filterLines.Where(line => line.Toggle == state).Select(line => line.Toggle).Count();
        }

        public List<FilterLine> GetLineToggle(bool state)
        {
            return filterLines.Where(line => line.Toggle == state).Select(line => line).ToList();
        }
    }
}