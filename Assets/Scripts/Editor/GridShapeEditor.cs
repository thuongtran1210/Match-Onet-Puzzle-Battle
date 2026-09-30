using UnityEditor;
using UnityEngine;
using BeastLinkBattle.Grid.Data;

[CustomEditor(typeof(GridShape))]
public class GridShapeEditor : Editor
{
    private GridShape shape;

    private const int cellSize = 30;


    private void OnEnable()
    {
        shape = (GridShape)target;

        if (shape.cells == null || shape.cells.Length != shape.width * shape.height)
        {
            Resize();
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Resize Grid"))
        {
            Resize();
        }

        GUILayout.Space(10);

        DrawGrid();
    }

    private void Resize()
    {
        shape.cells = new GridShapeCell[shape.width * shape.height];

        for (int i = 0; i < shape.cells.Length; i++)
        {
            shape.cells[i] = GridShapeCell.Normal;
        }

        EditorUtility.SetDirty(shape);
    }

    private void DrawGrid()
    {
        for (int y = 0; y < shape.height; y++)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < shape.width; x++)
            {
                var cell = shape.GetCell(x, y);

                GUI.backgroundColor = GetColor(cell);

                if (GUILayout.Button("", GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                {
                    shape.SetCell(x, y, NextState(cell));
                    EditorUtility.SetDirty(shape);
                }
            }

            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }

    private GridShapeCell NextState(GridShapeCell current)
    {
        switch (current)
        {
            case GridShapeCell.Normal:
                return GridShapeCell.Invalid;

            case GridShapeCell.Invalid:
                return GridShapeCell.Blocked;

            case GridShapeCell.Blocked:
                return GridShapeCell.Normal;

            default:
                return GridShapeCell.Normal;
        }
    }

    private Color GetColor(GridShapeCell cell)
    {
        switch (cell)
        {
            case GridShapeCell.Normal:
                return new Color(0.8f, 1f, 0.8f); // xanh lá nh?t

            case GridShapeCell.Invalid:
                return new Color(0.2f, 0.2f, 0.2f); // xám d?m (d? g?t hon den)

            case GridShapeCell.Blocked:
                return new Color(1f, 0.6f, 0.6f); // d? nh?t

            default:
                return Color.white;
        }
    }
}