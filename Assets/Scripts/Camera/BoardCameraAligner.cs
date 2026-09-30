using UnityEngine;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.Grid.Service;

public class BoardCameraAligner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BoardController boardController;
    [SerializeField] private Camera cam;
    [SerializeField] private float cellSize = 1.1f;
    [SerializeField] private float padding = 1.0f; // L? 2 bên và bên du?i

    [Header("Battle Area Settings")]
    [Tooltip("Chi?u cao c?a khu v?c chi?n d?u phía trên b?ng")]
    [SerializeField] private float battleAreaHeight = 6.0f;

    private void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        boardController.OnBoardDataReady += AlignCamera;
    }

    private void OnDisable()
    {
        boardController.OnBoardDataReady -= AlignCamera;
    }

    [ContextMenu("Align Now")]
    public void AlignCamera()
    {
        IGridService gridService = boardController.GetGridService();
        if (gridService == null || cam == null) return;

        int width = gridService.Width;
        int height = gridService.Height;

        // 1. TÍNH TOÁN V? TRÍ TÂM M?I (D?ch lên trên d? l?y c? Battle Area)
        float boardWidth = width * cellSize;
        float boardHeight = height * cellSize;

        float centerX = (width - 1) * cellSize * 0.5f;
        // Tâm Y m?i s? c?ng thêm m?t n?a chi?u cao c?a khu v?c Battle
        float centerY = (boardHeight * 0.5f) + (battleAreaHeight * 0.5f) - (cellSize * 0.5f);

        Vector3 boardOrigin = boardController.transform.position;
        Vector3 targetPos = new Vector3(boardOrigin.x + centerX, boardOrigin.y + centerY, -10f);

        transform.position = targetPos;

        // 2. TÍNH TOÁN ZOOM (ORTHOGRAPHIC SIZE)
        // Chi?u cao t?ng b?ng b?ng + battle area + padding 2 d?u
        float totalHeight = boardHeight + battleAreaHeight + (padding * 2);
        float totalWidth = boardWidth + (padding * 2);

        float screenAspect = (float)Screen.width / (float)Screen.height;

        float orthoSizeByHeight = totalHeight * 0.5f;
        float orthoSizeByWidth = (totalWidth / screenAspect) * 0.5f;

        // Ch?n giá tr? l?n nh?t d? bao quát du?c toàn b?
        cam.orthographicSize = Mathf.Max(orthoSizeByHeight, orthoSizeByWidth);

    }
}