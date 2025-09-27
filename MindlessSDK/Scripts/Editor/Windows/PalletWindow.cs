using UnityEditor;
using UnityEngine;

namespace MindlessSDK.Editor
{
    public class PallletWindow : EditorWindow
    {
        public const string titleDummy = "title";
        public static string palletTitle = "New Pallet";
        private static string _palletBarcode = "Barcode";
        public static string palletBarcode => _palletBarcode.ToLower().Replace(" ", "");
        public static string palletAuthor = "Author";
        public static string palletVersion = "1.0";
        public static string path => $"Assets/SDK/pallets/{palletBarcode}";
        public static Pallet selectedPallet;
        [MenuItem("MindlessSDK/Editor/Pallet/Editor")]
        public static void OpenWindow()
        {
            PallletWindow window = GetWindow<PallletWindow>("Pallet Editor");
            window.Show();
        }
        public static bool NameError(string victim, string enemy)
        {
            bool error = victim == enemy || string.IsNullOrEmpty(victim) || string.IsNullOrWhiteSpace(victim);
            if(error) EditorGUILayout.HelpBox($"Name cannot be: {victim}", MessageType.Error, true);
            return error;
        }
        public void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Pallet Settings", EditorStyles.boldLabel);
            palletTitle = EditorGUILayout.TextField("Pallet Title", palletTitle);
            if (NameError(palletTitle, titleDummy))return;
            palletAuthor = EditorGUILayout.TextField("Author", palletAuthor);
            palletVersion = EditorGUILayout.TextField("Version", palletVersion);

            if (GUILayout.Button("Create Pallet"))
            {
                CreatePallet();
            }
        }

        public static void CreatePallet()
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            _palletBarcode = $"{palletAuthor}.{palletTitle}";

            Pallet newPallet = CreateInstance<Pallet>();
            newPallet.Title = palletTitle;
            newPallet.Barcode = palletBarcode;
            newPallet.Author = palletAuthor;
            newPallet.Version = palletVersion;

            AssetDatabase.CreateAsset(newPallet, path + "/" + newPallet.Barcode + ".pallet.asset");
            //AddPalletToAddressables(newPallet);
            AssetDatabase.SaveAssets();
            selectedPallet = newPallet;
            EditorUtility.DisplayDialog("Pallet Created", $"Pallet {newPallet.Title} has been created for pallet {selectedPallet.Title}.", "OK");
            palletTitle = titleDummy;
        }

        public void SelectPallet()
        {
            string path = "Assets/SDK/pallets";
            string[] palletGuids = AssetDatabase.FindAssets("t:Pallet", new[] { path });

            GenericMenu menu = new GenericMenu();

            foreach (string guid in palletGuids)
            {
                string palletPath = AssetDatabase.GUIDToAssetPath(guid);
                Pallet pallet = AssetDatabase.LoadAssetAtPath<Pallet>(palletPath);
                menu.AddItem(new GUIContent(pallet.Title), false, () => SelectPalletFromMenu(pallet));
            }

            menu.ShowAsContext();
        }

        public void SelectPalletFromMenu(Pallet pallet)
        {
            selectedPallet = pallet;
        }
    }
}