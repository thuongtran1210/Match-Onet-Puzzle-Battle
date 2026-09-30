// Scripts/Editor/BeastDataGenerator.cs
using UnityEngine;
using UnityEditor;
using BeastLinkBattle.Gameplay.Data; // Ð?m b?o namespace này kh?p v?i BeastDefinition c?a b?n

namespace BeastLinkBattle.EditorTools
{
    public class BeastDataGenerator
    {
        // T?o m?t menu m?i trên thanh công c? c?a Unity
        [MenuItem("Tools/Generate Default Beasts")]
        public static void GenerateBeasts()
        {
            // 1. Khai báo du?ng d?n luu tr?. S? t? t?o thu m?c n?u chua có.
            string folderPath = "Assets/GameData/Beasts";

            if (!AssetDatabase.IsValidFolder("Assets/GameData"))
            {
                AssetDatabase.CreateFolder("Assets", "GameData");
            }
            if (!AssetDatabase.IsValidFolder("Assets/GameData/Beasts"))
            {
                AssetDatabase.CreateFolder("Assets/GameData", "Beasts");
            }

            // 2. Kh?i t?o m?t lo?t thú v?i các ch? s? co b?n
            // (B?n có th? thêm vòng l?p d?c t? file CSV ? dây)
            CreateBeastFile(folderPath, "FireDragon", ElementType.Fire, RoleType.Tanker, Color.red, 500, 40, 1.8f);
            CreateBeastFile(folderPath, "WaterSpirit", ElementType.Water, RoleType.Mage, Color.blue, 200, 80, 2.5f);
            CreateBeastFile(folderPath, "NatureEnt", ElementType.Nature, RoleType.Tanker, Color.green, 800, 25, 1.5f);
            CreateBeastFile(folderPath, "LightAssassin", ElementType.Light, RoleType.Assassin, Color.yellow, 300, 120, 3.0f);

            // 3. Luu và làm m?i Asset Database
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[BeastDataGenerator] Ðã t?o xong d? li?u thú t?i du?ng d?n: {folderPath}");
        }

        private static void CreateBeastFile(string folderPath, string idName, ElementType element, RoleType role, Color uiColor, int baseHp, int baseDamage, float moveSpeed)
        {
            // Bu?c A: T?o m?t Instance c?a ScriptableObject trong b? nh?
            BeastDefinition newBeast = ScriptableObject.CreateInstance<BeastDefinition>();

            // Bu?c B: Gán d? li?u cho các tru?ng
            newBeast.beastId = idName;
            newBeast.element = element;
            newBeast.role = role;
            newBeast.uiColor = uiColor;

            newBeast.baseHp = baseHp;
            newBeast.baseDamage = baseDamage;
            newBeast.moveSpeed = moveSpeed;
            newBeast.attackRange = 1.2f; // C? d?nh ho?c truy?n vào tham s?
            newBeast.attackCooldown = 1.0f;

            // Bu?c C: Xác d?nh tên file và d?m b?o tên không b? trùng
            // Hàm GenerateUniqueAssetPath s? t? d?ng thêm s? (vd: Beast_FireDragon 1.asset) n?u file dã t?n t?i
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/Beast_{idName}.asset");

            // Bu?c D: Luu Instance t? b? nh? xu?ng ? c?ng thành file .asset
            AssetDatabase.CreateAsset(newBeast, assetPathAndName);
        }
    }
}