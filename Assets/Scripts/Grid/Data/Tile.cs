using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UIElements;

namespace BeastLinkBattle.Grid.Data
{
    public class Tile
    {
        public Vector2Int Position { get; private set; }
        public TileState State { get; private set; }
        public ITileContent Content { get; private set; }       

        public Tile(Vector2Int pos, TileState state)
        {
            Position = pos;
            State = state;
            Content = null;
        }

        public void SetContent(ITileContent newContent)
        {
            Content = newContent;
            State = TileState.Occupied;
        }

        public void ClearContent()
        {
            Content = null;
            State = TileState.Empty;
        }
        public void SetBlockState(bool isBlocked)
        {
            if (Content != null) return; 
            State = isBlocked ? TileState.Blocked : TileState.Empty;
        }
        public void SetState(TileState newState)
        {
            if (newState == TileState.Empty || newState == TileState.Blocked)
            {
                Content = null;
            }

            State = newState;
        }
    }

}