// TileType đại diện cho loại nội dung có thể tồn tại trên một ô trong grid. Các loại này có thể bao gồm:
// - None: Không có nội dung nào, ô trống.  
// - Beast: Ô chứa một con quái vật, có thể tương tác hoặc chiến đấu.
// - Energy: Ô chứa năng lượng, có thể được thu thập để tăng điểm hoặc cung cấp năng lượng cho các hành động khác.
public enum TileType
{
    None,
    Beast,
    Energy
}