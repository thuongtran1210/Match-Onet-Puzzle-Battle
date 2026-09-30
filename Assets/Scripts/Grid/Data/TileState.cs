// TileState đại diện cho trạng thái của một ô trong grid, ảnh hưởng đến cách nó tương tác với nội dung và người chơi. Các trạng thái bao gồm:
// - Empty: Ô trống, có thể đặt nội dung mới vào.
// - Occupied: Ô đã có nội dung, không thể đặt thêm nội dung mới cho đến khi nó được làm rỗng.
// - Blocked: Ô bị khóa, không thể đi qua hoặc đặt nội dung vào.
// - Invalid: Ô không tồn tại, thường dùng để đánh dấu các vị trí ngoài biên của grid hoặc các ô bị loại bỏ.
// - Border: Ô biên, có thể được sử dụng để tạo hiệu ứng hoặc giới hạn di chuyển mà không phải là ô chơi được.

public enum TileState
{
    Empty,
    Occupied,
    Blocked,
    Invalid,
    Border
}