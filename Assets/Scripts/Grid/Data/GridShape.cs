
using UnityEngine;

namespace BeastLinkBattle.Grid.Data
{
    [CreateAssetMenu(menuName = "Grid/Grid Shape")]
    public class GridShape : ScriptableObject
    {
        public int width;
        public int height;

        [Tooltip("Flatten array: width * height")]
        public GridShapeCell[] cells;

        public GridShapeCell GetCell(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return GridShapeCell.Invalid;
            int index = y * width + x;
            return cells[index];
        }
        public void SetCell(int x, int y, GridShapeCell value)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return;

            int index = y * width + x;
            cells[index] = value;
        }
    }
}