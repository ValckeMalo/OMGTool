namespace MVProduction.EditorCode
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class EditorGrid : IDisposable
    {
        private int columns;
        private int rows;
        private int currentColumn;
        private int currentRow;

        /// <summary>
        /// Initializes a grid with the specified number of columns and rows.
        /// </summary>
        /// <param name="columns">Number of columns.</param>
        /// <param name="rows">Number of rows (optional).</param>
        /// <param name="isScrollable">Enable or disable scrolling.</param>
        /// <param name="style">Custom style for the entire grid (optional).</param>
        public EditorGrid(int columns, int rows = -1, GUIStyle style = null)
        {
            this.columns = columns;
            this.rows = rows;
            this.currentColumn = 0;
            this.currentRow = 0;

            // Begin the vertical container for the grid with optional style
            GUILayout.BeginVertical(style ?? GUIStyle.none);
            EditorGUILayout.BeginHorizontal(); // Start the first row
        }

        /// <summary>
        /// Adds a new cell to the grid.
        /// </summary>
        /// <param name="action">Content to render inside the cell.</param>
        public void AddCell(Action action)
        {
            if (currentRow >= rows && rows > 0) return; // Stop if we exceed the row limit

            // Start a new row if the column limit is reached
            if (currentColumn >= columns)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(); // Begin a new row
                currentColumn = 0;
                currentRow++;
            }

            if (currentRow < rows || rows <= 0)
            {
                // Add the cell's content
                action?.Invoke();
                currentColumn++;
            }
        }

        /// <summary>
        /// Ends the grid layout and cleans up when disposed.
        /// </summary>
        public void Dispose()
        {
            // End the last row
            EditorGUILayout.EndHorizontal();

            // End the vertical container for the grid
            GUILayout.EndVertical();
        }
    }
}
