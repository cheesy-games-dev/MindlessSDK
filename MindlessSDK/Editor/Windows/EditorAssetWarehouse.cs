using System.Linq;
using MindlessSDK;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorAssetWarehouse : EditorWindow
{
    public static string mainPath = $"Assets/_MindlessAssets";
    public const string palletsKey = "_Pallets";
    public const string preivewsMeshesKey = "_PreviewMeshes";
    public static string palletsPath = $"{mainPath}/{palletsKey}";
    public static string preivewsMeshesPath = $"{mainPath}/{preivewsMeshesKey}";

    [MenuItem("MindlessSDK/Asset Warehouse")]
    public static void ShowExample()
    {
        EditorAssetWarehouse wnd = GetWindow<EditorAssetWarehouse>();
        wnd.titleContent = new GUIContent("Asset Warehouse");
    }
    public Vector2 scrollPos = Vector2.up;
    public Pallet CurrentPallet = null;
    public void OnGUI()
    {
        GUILayout.Label("Asset Warehouse", EditorStyles.boldLabel);
        GUILayout.BeginVertical();
        AssetDatabase.FindAssets("t:Pallet", new[] { $"{palletsPath}" })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<Pallet>)
            .ToList()
            .ForEach(s =>
            {
                GUILayout.Label(s.Title);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button($"Select"))
                {
                    Selection.activeObject = s;
                }
                if (GUILayout.Button($"Open"))
                {
                    CurrentPallet = s;
                }
                GUILayout.EndHorizontal();
            });
        if (CurrentPallet)
        {
            GUILayout.Space(20);
            GUILayout.Label($"Current Pallet: {CurrentPallet.Title}", EditorStyles.boldLabel);
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);
            CurrentPallet.Crates.ToList().ForEach(c =>
            {
                if (c == null) return;
                GUILayout.Label(c.Title);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button($"Select"))
                {
                    Selection.activeObject = c;
                }
                if (c is SpawnableCrate)
                {
                    if (GUILayout.Button($"Create CrateSpawner"))
                    {
                        SpawnableCrate.CreateCrateSpawner(c as SpawnableCrate);
                    }
                }
                else if (c is LevelCrate)
                {
                    if (GUILayout.Button($"Open Level"))
                    {
                        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(c.CrateReference));
                    }
                }
                GUILayout.EndHorizontal();
            });
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }
}
