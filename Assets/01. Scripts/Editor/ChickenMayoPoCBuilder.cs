// 치킨마요 PoC용 데이터, 프리팹, 그레이박스 씬을 한 번에 생성하는 에디터 메뉴
using System;
using System.Linq;
using Dosirak.Cooking;
using Dosirak.Data;
using Dosirak.Orders;
using Dosirak.Player;
using Dosirak.PoC;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Dosirak.EditorTools
{
    public static class ChickenMayoPoCBuilder
    {
        private const string DataDir = "Assets/02. Data/PoC";
        private const string PrefabDir = "Assets/03. Prefabs/PoC";
        private const string MaterialDir = "Assets/07. Materials/PoC";
        private const string SceneDir = "Assets/04. Scenes/PoC";
        private const string ScenePath = SceneDir + "/PoC_ChickenMayo.unity";
        private const string InputPath = "Assets/02. Data/Input/PlayerControls.inputactions";
        private const float CounterTop = 0.9f;

        private static readonly Color FloorColor = new Color(0.43f, 0.41f, 0.38f);
        private static readonly Color WallColor = new Color(0.94f, 0.91f, 0.85f);
        private static readonly Color SteelColor = new Color(0.72f, 0.74f, 0.75f);
        private static readonly Color WoodColor = new Color(0.78f, 0.58f, 0.37f);

        [MenuItem("Dosirak/PoC/Build Chicken Mayo Scene")]
        public static void Build()
        {
            foreach (var dir in new[] { DataDir, PrefabDir, MaterialDir, SceneDir }) EnsureFolder(dir);
            // 새 씬을 열면 미리 불러온 프리팹 참조가 끊기므로 가장 먼저 연다.
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cube = PrimitiveMesh(PrimitiveType.Cube);
            var cylinder = PrimitiveMesh(PrimitiveType.Cylinder);
            var capsule = PrimitiveMesh(PrimitiveType.Capsule);

            // 데이터
            var fry = LoadOrCreate<CookProfileData>("CP_Fry");
            var sliced = ItemAsset("ID_ChickenSliced", "치킨 슬라이스", cube, new Vector3(0.12f, 0.02f, 0.12f), Color.white, true, false);
            var chicken = ItemAsset("ID_Chicken", "치킨", capsule, new Vector3(0.07f, 0.05f, 0.07f), Color.white, true, false);
            Set(chicken, "cookProfile", fry);
            Set(chicken, "slicedResult", sliced);
            var rice = ItemAsset("ID_Rice", "밥", cube, new Vector3(0.14f, 0.03f, 0.14f), new Color(0.95f, 0.93f, 0.89f), true, true);
            var egg = ItemAsset("ID_Egg", "계란지단", cube, new Vector3(0.13f, 0.01f, 0.13f), new Color(0.95f, 0.70f, 0.20f), false, false);
            var lid = ItemAsset("ID_Lid", "뚜껑", cylinder, new Vector3(0.17f, 0.005f, 0.17f), new Color(0.90f, 0.90f, 0.88f), false, false);
            var sauce = ItemAsset("ID_Sauce", "소스", cylinder, new Vector3(0.04f, 0.02f, 0.04f), new Color(0.45f, 0.25f, 0.10f), false, false);
            var spoon = ItemAsset("ID_Spoon", "숟가락", cube, new Vector3(0.03f, 0.01f, 0.15f), SteelColor, false, false);
            var chopsticks = ItemAsset("ID_Chopsticks", "젓가락", cube, new Vector3(0.02f, 0.01f, 0.20f), WoodColor, false, false);
            var wipe = ItemAsset("ID_Wipe", "물티슈", cube, new Vector3(0.07f, 0.01f, 0.10f), new Color(0.94f, 0.94f, 0.92f), false, false);
            var bowlData = ItemAsset("ID_Bowl", "보울", cylinder, new Vector3(0.18f, 0.03f, 0.18f), new Color(0.11f, 0.11f, 0.10f), false, false);
            var bagData = ItemAsset("ID_Bag", "봉투", cube, new Vector3(0.25f, 0.30f, 0.15f), new Color(0.94f, 0.92f, 0.86f), false, false);

            var menu = LoadOrCreate<MenuData>("MD_ChickenMayo");
            Set(menu, "displayName", "치킨마요");
            SetObjectList(menu, "steps", rice, egg, sliced, lid, sauce);
            SetPackaging(menu, (spoon, 1), (chopsticks, 1), (wipe, 1));

            // 프리팹
            var baseMaterial = MaterialFor(Color.white);
            var itemPrefab = ItemPrefab("P_Item", baseMaterial, null);
            var bowlPrefab = ItemPrefab("P_Bowl", baseMaterial, root => Set(root.AddComponent<BowlContainer>(), "menu", menu));
            var bagPrefab = ItemPrefab("P_Bag", baseMaterial, root => root.AddComponent<BagContainer>());

            // 씬
            var lightGo = new GameObject("Directional Light");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.color = new Color(1f, 0.96f, 0.90f);
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            Box("바닥", new Vector3(0f, -0.05f, 0f), new Vector3(8f, 0.1f, 8f), FloorColor);
            Box("뒷벽", new Vector3(0f, 1.25f, 2.3f), new Vector3(8f, 2.5f, 0.1f), WallColor);
            Box("앞벽 왼쪽", new Vector3(-2.3f, 1.25f, -2.3f), new Vector3(3.4f, 2.5f, 0.1f), WallColor);
            Box("앞벽 오른쪽", new Vector3(2.3f, 1.25f, -2.3f), new Vector3(3.4f, 2.5f, 0.1f), WallColor);
            Box("앞벽 위", new Vector3(0f, 2.25f, -2.3f), new Vector3(1.2f, 0.5f, 0.1f), WallColor);
            Box("주문대", new Vector3(0f, CounterTop / 2f, -2.3f), new Vector3(1.2f, CounterTop, 0.5f), WoodColor);
            Box("뒤 조리대", new Vector3(0f, CounterTop / 2f, 1.8f), new Vector3(6f, CounterTop, 0.7f), SteelColor);
            Box("작업대", new Vector3(0f, CounterTop / 2f, 0.2f), new Vector3(1.6f, CounterTop, 0.7f), SteelColor);
            Box("포장대", new Vector3(2.9f, CounterTop / 2f, -0.6f), new Vector3(0.7f, CounterTop, 2.2f), WoodColor);

            DispenserBox("생닭 상자", -2.6f, 1.8f, new Vector3(0.4f, 0.15f, 0.35f), new Color(0.85f, 0.75f, 0.65f), itemPrefab, chicken);
            FryerBox(-1.8f, 1.8f);
            SlicerBox(-1.0f, 1.8f);
            DispenserBox("밥솥", -0.2f, 1.8f, new Vector3(0.4f, 0.35f, 0.4f), new Color(0.92f, 0.90f, 0.86f), itemPrefab, rice);
            DispenserBox("지단 통", 0.5f, 1.8f, new Vector3(0.3f, 0.1f, 0.25f), new Color(0.95f, 0.70f, 0.20f), itemPrefab, egg);
            DispenserBox("보울 더미", 1.2f, 1.8f, new Vector3(0.22f, 0.2f, 0.22f), new Color(0.11f, 0.11f, 0.10f), bowlPrefab, bowlData);
            DispenserBox("뚜껑 더미", 1.8f, 1.8f, new Vector3(0.22f, 0.1f, 0.22f), new Color(0.90f, 0.90f, 0.88f), itemPrefab, lid);
            DispenserBox("소스 통", 2.5f, 1.8f, new Vector3(0.15f, 0.2f, 0.15f), new Color(0.45f, 0.25f, 0.10f), itemPrefab, sauce);
            DispenserBox("숟가락 통", 2.9f, 0.2f, new Vector3(0.2f, 0.15f, 0.2f), SteelColor, itemPrefab, spoon);
            DispenserBox("젓가락 통", 2.9f, -0.2f, new Vector3(0.2f, 0.15f, 0.2f), WoodColor, itemPrefab, chopsticks);
            DispenserBox("물티슈 상자", 2.9f, -0.6f, new Vector3(0.25f, 0.1f, 0.2f), new Color(0.94f, 0.94f, 0.92f), itemPrefab, wipe);
            DispenserBox("봉투 더미", 2.9f, -1.2f, new Vector3(0.3f, 0.08f, 0.25f), new Color(0.94f, 0.92f, 0.86f), bagPrefab, bagData);

            var customerGo = Box("손님", new Vector3(0f, 0.85f, -3.0f), new Vector3(0.5f, 1.7f, 0.35f), new Color(0.55f, 0.45f, 0.40f));
            var customer = customerGo.AddComponent<Customer>();
            Set(customer, "order", menu);

            var labelNames = new[] { "주문대", "작업대", "포장대", "생닭 상자", "튀김기", "치킨 슬라이서", "밥솥", "지단 통", "보울 더미", "뚜껑 더미", "소스 통", "숟가락 통", "젓가락 통", "물티슈 상자", "봉투 더미", "손님" };
            var labels = labelNames.Select(n => (Object)GameObject.Find(n).GetComponent<Collider>()).ToArray();
            BuildPlayer(customer, labels);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log($"치킨마요 PoC 씬을 생성했다. {ScenePath}");
        }

        private static void BuildPlayer(Customer customer, Object[] labels)
        {
            var input = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputPath);
            var player = new GameObject("Player");
            // 시선 레이캐스트가 플레이어 자신의 충돌체에 맞지 않도록 한다.
            player.layer = LayerMask.NameToLayer("Ignore Raycast");
            player.transform.position = new Vector3(0f, 0f, -1f);
            var characterController = player.AddComponent<CharacterController>();
            characterController.height = 1.8f;
            characterController.radius = 0.3f;
            characterController.center = new Vector3(0f, 0.9f, 0f);

            var cameraRoot = new GameObject("CameraRoot");
            cameraRoot.tag = "MainCamera";
            cameraRoot.transform.SetParent(player.transform, false);
            cameraRoot.transform.localPosition = new Vector3(0f, 1.65f, 0f);
            var cam = cameraRoot.AddComponent<Camera>();
            cam.nearClipPlane = 0.03f;
            cam.fieldOfView = 70f;
            cameraRoot.AddComponent<AudioListener>();

            var holdPoint = new GameObject("HoldPoint");
            holdPoint.transform.SetParent(cameraRoot.transform, false);
            holdPoint.transform.localPosition = new Vector3(0.25f, -0.22f, 0.5f);

            var controller = player.AddComponent<PlayerController>();
            Set(controller, "actions", input);
            Set(controller, "cameraRoot", cameraRoot.transform);

            var interactor = player.AddComponent<PlayerInteractor>();
            Set(interactor, "actions", input);
            Set(interactor, "cam", cam);
            Set(interactor, "holdPoint", holdPoint.transform);
            Set(interactor, "controller", controller);

            var hud = player.AddComponent<PoCHud>();
            Set(hud, "interactor", interactor);
            Set(hud, "customer", customer);
            Set(hud, "cam", cam);
            SetObjectList(hud, "labelTargets", labels);
        }

        private static void DispenserBox(string name, float x, float z, Vector3 size, Color color, Item prefab, ItemData data)
        {
            var go = Box(name, new Vector3(x, CounterTop + size.y / 2f, z), size, color);
            var dispenser = go.AddComponent<Dispenser>();
            Set(dispenser, "itemPrefab", prefab);
            Set(dispenser, "itemData", data);
        }

        private static void FryerBox(float x, float z)
        {
            var size = new Vector3(0.55f, 0.35f, 0.5f);
            var go = Box("튀김기", new Vector3(x, CounterTop + size.y / 2f, z), size, SteelColor);
            var basket = new GameObject("바구니");
            basket.transform.SetParent(go.transform, false);
            basket.transform.localPosition = new Vector3(0f, size.y / 2f + 0.03f, 0f);
            Visual(basket.transform, new Vector3(0.4f, 0.03f, 0.35f), new Color(0.25f, 0.25f, 0.25f));
            Set(go.AddComponent<Fryer>(), "basket", basket.transform);
        }

        private static void SlicerBox(float x, float z)
        {
            var size = new Vector3(0.3f, 0.3f, 0.3f);
            var go = Box("치킨 슬라이서", new Vector3(x, CounterTop + size.y / 2f, z), size, new Color(0.60f, 0.62f, 0.64f));
            var slot = new GameObject("Slot");
            slot.transform.SetParent(go.transform, false);
            slot.transform.localPosition = new Vector3(0f, size.y / 2f + 0.03f, 0f);
            Set(go.AddComponent<Slicer>(), "slot", slot.transform);
        }

        // 충돌체는 루트에, 크기는 자식 Visual에 둔다. 루트가 스케일되지 않아야 올려놓은 물건이 찌그러지지 않는다.
        private static GameObject Box(string name, Vector3 position, Vector3 size, Color color)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.AddComponent<BoxCollider>().size = size;
            Visual(go.transform, size, color);
            return go;
        }

        private static void Visual(Transform parent, Vector3 size, Color color)
        {
            var visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = "Visual";
            Object.DestroyImmediate(visual.GetComponent<Collider>());
            visual.transform.SetParent(parent, false);
            visual.transform.localScale = size;
            visual.GetComponent<MeshRenderer>().sharedMaterial = MaterialFor(color);
        }

        private static Item ItemPrefab(string name, Material material, Action<GameObject> extra)
        {
            var root = new GameObject(name);
            root.AddComponent<BoxCollider>();
            var visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = "Visual";
            Object.DestroyImmediate(visual.GetComponent<Collider>());
            visual.transform.SetParent(root.transform, false);
            visual.GetComponent<MeshRenderer>().sharedMaterial = material;
            Set(root.AddComponent<Item>(), "visual", visual.transform);
            extra?.Invoke(root);
            var path = $"{PrefabDir}/{name}.prefab";
            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            // 저장 직후 반환값은 재임포트로 끊길 수 있어 경로에서 다시 불러온다.
            return AssetDatabase.LoadAssetAtPath<GameObject>(path).GetComponent<Item>();
        }

        private static ItemData ItemAsset(string assetName, string displayName, Mesh mesh, Vector3 scale, Color color, bool usesTemperature, bool startsHot)
        {
            var data = LoadOrCreate<ItemData>(assetName);
            Set(data, "displayName", displayName);
            Set(data, "mesh", mesh);
            Set(data, "scale", scale);
            Set(data, "color", color);
            Set(data, "usesTemperature", usesTemperature);
            Set(data, "startsHot", startsHot);
            return data;
        }

        private static Material MaterialFor(Color color)
        {
            var path = $"{MaterialDir}/M_PoC_{ColorUtility.ToHtmlStringRGB(color)}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.SetColor("_BaseColor", color);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static T LoadOrCreate<T>(string assetName) where T : ScriptableObject
        {
            var path = $"{DataDir}/{assetName}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static Mesh PrimitiveMesh(PrimitiveType type)
        {
            var go = GameObject.CreatePrimitive(type);
            var mesh = go.GetComponent<MeshFilter>().sharedMesh;
            Object.DestroyImmediate(go);
            return mesh;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var slash = path.LastIndexOf('/');
            var parent = path.Substring(0, slash);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, path.Substring(slash + 1));
        }

        private static void Set(Object target, string field, object value)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(field);
            switch (value)
            {
                case Object obj: p.objectReferenceValue = obj; break;
                case string s: p.stringValue = s; break;
                case bool b: p.boolValue = b; break;
                case int i: p.intValue = i; break;
                case float f: p.floatValue = f; break;
                case Color c: p.colorValue = c; break;
                case Vector3 v: p.vector3Value = v; break;
                default: throw new ArgumentException($"지원하지 않는 값 타입이다. {field}");
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        private static void SetObjectList(Object target, string field, params Object[] values)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(field);
            p.arraySize = values.Length;
            for (var i = 0; i < values.Length; i++) p.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        private static void SetPackaging(MenuData menu, params (ItemData item, int count)[] entries)
        {
            var so = new SerializedObject(menu);
            var p = so.FindProperty("packaging");
            p.arraySize = entries.Length;
            for (var i = 0; i < entries.Length; i++)
            {
                var element = p.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("item").objectReferenceValue = entries[i].item;
                element.FindPropertyRelative("count").intValue = entries[i].count;
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(menu);
        }
    }
}
