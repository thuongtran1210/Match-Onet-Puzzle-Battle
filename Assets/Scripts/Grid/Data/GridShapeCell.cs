namespace BeastLinkBattle.Grid.Data
{
    public enum GridShapeCell
    {
        Invalid,  // Không tồn tại, không thể đi qua
        Normal,  // Có thể đi qua và spawn content
        Blocked   // Không thể đi qua, nhưng vẫn tồn tại trên grid (dùng để tạo chướng ngại vật hoặc vết cản)
    }
}