using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gera automaticamente todos os recursos e a cena principal do jogo.
/// Menu: Tools > Platformer > Generate Project
/// A execucao e idempotente: os recursos gerados sao recriados a cada execucao.
/// </summary>
public static class ProjectSetup
{
    private const string ScenePath = "Assets/Scenes/MainScene.unity";
    private const string MaterialsFolder = "Assets/Materials";
    private const string PrefabsFolder = "Assets/Prefabs";

    private const string PlayerMaterialPath = MaterialsFolder + "/PlayerMaterial.mat";
    private const string DirtMaterialPath = MaterialsFolder + "/DirtMaterial.mat";
    private const string GrassMaterialPath = MaterialsFolder + "/GrassMaterial.mat";
    private const string CoinMaterialPath = MaterialsFolder + "/CoinMaterial.mat";
    private const string BackgroundMaterialPath = MaterialsFolder + "/BackgroundMaterial.mat";
    private const string GoalMaterialPath = MaterialsFolder + "/GoalMaterial.mat";

    private const string PlayerPrefabPath = PrefabsFolder + "/Player.prefab";
    private const string PlatformPrefabPath = PrefabsFolder + "/Platform.prefab";
    private const string CoinPrefabPath = PrefabsFolder + "/Coin.prefab";

    private const float PlayerStartOffsetY = 1.1f;
    private const float FallThresholdY = -10f;

    private struct PlatformData
    {
        public readonly float X;
        public readonly float Top;
        public readonly float Width;
        public readonly float Height;
        public readonly float Depth;

        public PlatformData(float x, float top, float width, float height, float depth)
        {
            X = x;
            Top = top;
            Width = width;
            Height = height;
            Depth = depth;
        }
    }

    // 12 plataformas principais. "Top" representa a altura da superficie de apoio.
    private static readonly PlatformData[] Platforms =
    {
        new PlatformData(0f, 0f, 8f, 1f, 3f),
        new PlatformData(8f, 1.5f, 4f, 1f, 3f),
        new PlatformData(13f, 3f, 3f, 1f, 3f),
        new PlatformData(18f, 2f, 4f, 1f, 3f),
        new PlatformData(24f, 4f, 3f, 1f, 3f),
        new PlatformData(30f, 5.5f, 3f, 1f, 3f),
        new PlatformData(36f, 4f, 4f, 1f, 3f),
        new PlatformData(43f, 6f, 3f, 1f, 3f),
        new PlatformData(49f, 7f, 3f, 1f, 3f),
        new PlatformData(55f, 5.5f, 4f, 1f, 3f),
        new PlatformData(62f, 7.5f, 3f, 1f, 3f),
        new PlatformData(68f, 8f, 6f, 1f, 3f)
    };

    private static readonly Vector3[] CoinPositions =
    {
        new Vector3(-2f, 1f, 0f),
        new Vector3(2f, 1f, 0f),
        new Vector3(8f, 2.5f, 0f),
        new Vector3(13f, 4f, 0f),
        new Vector3(18f, 3f, 0f),
        new Vector3(24f, 5f, 0f),
        new Vector3(30f, 6.5f, 0f),
        new Vector3(36f, 5f, 0f),
        new Vector3(43f, 7f, 0f),
        new Vector3(49f, 8f, 0f),
        new Vector3(55f, 6.5f, 0f),
        new Vector3(62f, 8.5f, 0f)
    };

    [MenuItem("Tools/Platformer/Generate Project")]
    public static void GenerateProject()
    {
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("Pare o modo Play antes de gerar o projeto.");
            return;
        }

        EnsureFolder("Assets/Scenes");
        EnsureFolder(MaterialsFolder);
        EnsureFolder(PrefabsFolder);
        EnsureFolder("Assets/Editor");

        // Cria uma cena vazia antes de remover os recursos antigos.
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        DeleteGeneratedAssets();
        AssetDatabase.Refresh();

        Material playerMaterial = CreateMaterial(PlayerMaterialPath, "PlayerMaterial", new Color(0.12f, 0.42f, 0.95f));
        Material dirtMaterial = CreateMaterial(DirtMaterialPath, "DirtMaterial", new Color(0.45f, 0.30f, 0.16f));
        Material grassMaterial = CreateMaterial(GrassMaterialPath, "GrassMaterial", new Color(0.20f, 0.65f, 0.25f));
        Material coinMaterial = CreateMaterial(CoinMaterialPath, "CoinMaterial", new Color(1f, 0.82f, 0.12f));
        Material backgroundMaterial = CreateMaterial(BackgroundMaterialPath, "BackgroundMaterial", new Color(0.62f, 0.82f, 0.98f));
        Material goalMaterial = CreateMaterial(GoalMaterialPath, "GoalMaterial", new Color(0.15f, 1f, 0.35f, 0.35f), true);

        ConfigurePlayerSettings();
        ConfigureRenderSettings();
        CreateLighting();
        CreateBackdrop(backgroundMaterial);

        GameObject playerPrefab = CreatePlayerPrefab(playerMaterial);
        GameObject platformPrefab = CreatePlatformPrefab(dirtMaterial, grassMaterial);
        GameObject coinPrefab = CreateCoinPrefab(coinMaterial);

        GameObject player = InstantiatePrefab(
            playerPrefab,
            "Player",
            null,
            new Vector3(Platforms[0].X - Platforms[0].Width * 0.25f, Platforms[0].Top + PlayerStartOffsetY, 0f));

        CreateCamera(player.transform);

        GameObject platformsRoot = new GameObject("Platforms");
        CreatePlatforms(platformPrefab, platformsRoot.transform);

        GameObject coinsRoot = new GameObject("Coins");
        CreateCoins(coinPrefab, coinsRoot.transform);

        LevelManager levelManager = CreateGoal(Platforms[Platforms.Length - 1], player.transform, goalMaterial);
        GameManager gameManager = CreateGameManager();
        GameUI gameUI = CreateUI();
        gameManager.gameUI = gameUI;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, ScenePath);

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenePath, true)
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (levelManager != null)
        {
            Debug.Log("Projeto gerado com sucesso. Cena salva em " + ScenePath + ".");
        }
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        string parent = System.IO.Path.GetDirectoryName(folderPath);
        string folderName = System.IO.Path.GetFileName(folderPath);

        if (!string.IsNullOrEmpty(parent))
        {
            parent = parent.Replace('\\', '/');

            if (!AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }
        }

        AssetDatabase.CreateFolder(parent, folderName);
    }

    private static void DeleteGeneratedAssets()
    {
        string[] paths =
        {
            ScenePath,
            PlayerMaterialPath,
            DirtMaterialPath,
            GrassMaterialPath,
            CoinMaterialPath,
            BackgroundMaterialPath,
            GoalMaterialPath,
            PlayerPrefabPath,
            PlatformPrefabPath,
            CoinPrefabPath
        };

        foreach (string path in paths)
        {
            AssetDatabase.DeleteAsset(path);
        }
    }

    private static Material CreateMaterial(string assetPath, string materialName, Color color, bool transparent = false)
    {
        Shader shader = Shader.Find("Standard");

        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Lit");
        }

        if (shader == null)
        {
            shader = Shader.Find("Legacy Shaders/Diffuse");
        }

        if (shader == null)
        {
            Debug.LogError("Nenhum shader compativel foi encontrado.");
            return null;
        }

        Material material = new Material(shader);
        material.name = materialName;

        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", color);
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        if (material.HasProperty("_Metallic"))
        {
            material.SetFloat("_Metallic", 0.2f);
        }

        if (material.HasProperty("_Glossiness"))
        {
            material.SetFloat("_Glossiness", 0.55f);
        }

        if (transparent)
        {
            ConfigureTransparentMaterial(material, color);
        }

        AssetDatabase.CreateAsset(material, assetPath);
        return material;
    }

    private static void ConfigureTransparentMaterial(Material material, Color color)
    {
        if (!material.HasProperty("_Mode"))
        {
            return;
        }

        material.SetFloat("_Mode", 3f);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
        material.SetColor("_Color", color);
    }

    private static GameObject CreatePlayerPrefab(Material playerMaterial)
    {
        GameObject root = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        root.name = "Player";
        root.tag = "Player";
        root.layer = 2; // Ignore Raycast: evita que o proprio jogador seja detectado pelo GroundCheck.

        CapsuleCollider capsule = root.GetComponent<CapsuleCollider>();
        capsule.radius = 0.5f;
        capsule.height = 2f;
        capsule.center = Vector3.zero;
        capsule.direction = 1;

        ApplyMaterial(root, playerMaterial);

        Rigidbody body = root.AddComponent<Rigidbody>();
        body.mass = 1f;
        body.constraints = RigidbodyConstraints.FreezePositionZ |
                           RigidbodyConstraints.FreezeRotationX |
                           RigidbodyConstraints.FreezeRotationY |
                           RigidbodyConstraints.FreezeRotationZ;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        GameObject groundCheckObject = new GameObject("GroundCheck");
        groundCheckObject.transform.SetParent(root.transform, false);
        groundCheckObject.transform.localPosition = new Vector3(0f, -1f, 0f);

        GroundCheck groundCheck = groundCheckObject.AddComponent<GroundCheck>();
        groundCheck.groundMask = ~(1 << 2);
        groundCheck.castRadius = 0.35f;
        groundCheck.castDistance = 0.25f;

        PlayerMovement movement = root.AddComponent<PlayerMovement>();
        movement.groundCheck = groundCheck;
        movement.moveSpeed = 6f;
        movement.jumpForce = 9f;

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, PlayerPrefabPath);
        Object.DestroyImmediate(root);
        return prefab;
    }

    private static GameObject CreatePlatformPrefab(Material dirtMaterial, Material grassMaterial)
    {
        GameObject root = new GameObject("Platform");

        BoxCollider collider = root.AddComponent<BoxCollider>();
        collider.center = new Vector3(0f, 0.5f, 0f);
        collider.size = Vector3.one;

        CreateVisualPrimitive(
            PrimitiveType.Cube,
            "Dirt",
            root.transform,
            new Vector3(0f, 0.45f, 0f),
            new Vector3(1f, 0.9f, 1f),
            dirtMaterial);

        CreateVisualPrimitive(
            PrimitiveType.Cube,
            "Grass",
            root.transform,
            new Vector3(0f, 0.95f, 0f),
            new Vector3(1f, 0.1f, 1f),
            grassMaterial);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, PlatformPrefabPath);
        Object.DestroyImmediate(root);
        return prefab;
    }

    private static GameObject CreateCoinPrefab(Material coinMaterial)
    {
        GameObject root = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        root.name = "Coin";
        root.transform.localScale = Vector3.one * 0.55f;

        SphereCollider collider = root.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;

        ApplyMaterial(root, coinMaterial);
        root.AddComponent<Coin>();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, CoinPrefabPath);
        Object.DestroyImmediate(root);
        return prefab;
    }

    private static GameObject CreateVisualPrimitive(
        PrimitiveType primitiveType,
        string objectName,
        Transform parent,
        Vector3 localPosition,
        Vector3 localScale,
        Material material)
    {
        GameObject primitive = GameObject.CreatePrimitive(primitiveType);
        primitive.name = objectName;

        Collider collider = primitive.GetComponent<Collider>();

        if (collider != null)
        {
            Object.DestroyImmediate(collider);
        }

        primitive.transform.SetParent(parent, false);
        primitive.transform.localPosition = localPosition;
        primitive.transform.localScale = localScale;
        ApplyMaterial(primitive, material);

        return primitive;
    }

    private static void ApplyMaterial(GameObject target, Material material)
    {
        Renderer renderer = target.GetComponent<Renderer>();

        if (renderer != null && material != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private static void ConfigurePlayerSettings()
    {
        PlayerSettings.companyName = "Projeto Academico";
        PlayerSettings.productName = "Plataforma 3D";
        PlayerSettings.defaultScreenWidth = 1280;
        PlayerSettings.defaultScreenHeight = 720;
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        PlayerSettings.runInBackground = true;
    }

    private static void ConfigureRenderSettings()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.62f, 0.66f, 0.70f);
    }

    private static void CreateLighting()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light directionalLight = lightObject.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.color = Color.white;
        directionalLight.intensity = 1.1f;
        directionalLight.shadows = LightShadows.Soft;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    private static void CreateBackdrop(Material backgroundMaterial)
    {
        GameObject backdrop = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backdrop.name = "Backdrop";

        Collider collider = backdrop.GetComponent<Collider>();

        if (collider != null)
        {
            Object.DestroyImmediate(collider);
        }

        backdrop.transform.position = new Vector3(34f, 10f, 8f);
        backdrop.transform.localScale = new Vector3(140f, 50f, 0.5f);
        ApplyMaterial(backdrop, backgroundMaterial);
    }

    private static GameObject InstantiatePrefab(GameObject prefab, string instanceName, Transform parent, Vector3 position)
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = instanceName;

        if (parent != null)
        {
            instance.transform.SetParent(parent, false);
        }

        instance.transform.position = position;
        return instance;
    }

    private static void CreatePlatforms(GameObject platformPrefab, Transform parent)
    {
        for (int i = 0; i < Platforms.Length; i++)
        {
            PlatformData data = Platforms[i];

            GameObject platform = (GameObject)PrefabUtility.InstantiatePrefab(platformPrefab);
            platform.name = "Platform_" + (i + 1);
            platform.transform.SetParent(parent, false);
            platform.transform.localPosition = new Vector3(data.X, data.Top - data.Height, 0f);
            platform.transform.localScale = new Vector3(data.Width, data.Height, data.Depth);
        }
    }

    private static void CreateCoins(GameObject coinPrefab, Transform parent)
    {
        for (int i = 0; i < CoinPositions.Length; i++)
        {
            GameObject coin = (GameObject)PrefabUtility.InstantiatePrefab(coinPrefab);
            coin.name = "Coin_" + (i + 1);
            coin.transform.SetParent(parent, false);
            coin.transform.localPosition = CoinPositions[i];
        }
    }

    private static LevelManager CreateGoal(PlatformData lastPlatform, Transform player, Material goalMaterial)
    {
        GameObject goal = new GameObject("Goal");
        goal.transform.position = new Vector3(lastPlatform.X + 1.5f, lastPlatform.Top + 1.25f, 0f);

        BoxCollider trigger = goal.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(2f, 2.5f, 4f);

        LevelManager levelManager = goal.AddComponent<LevelManager>();
        levelManager.player = player;
        levelManager.fallThresholdY = FallThresholdY;

        CreateVisualPrimitive(
            PrimitiveType.Cube,
            "GoalMarker",
            goal.transform,
            Vector3.zero,
            new Vector3(1.5f, 2f, 2.5f),
            goalMaterial);

        return levelManager;
    }

    private static GameManager CreateGameManager()
    {
        GameObject gameManagerObject = new GameObject("GameManager");
        return gameManagerObject.AddComponent<GameManager>();
    }

    private static Camera CreateCamera(Transform player)
    {
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 7.5f;
        camera.nearClipPlane = 0.3f;
        camera.farClipPlane = 200f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.55f, 0.80f, 1f);

        cameraObject.AddComponent<AudioListener>();

        CameraFollow follow = cameraObject.AddComponent<CameraFollow>();
        follow.target = player;
        follow.offset = new Vector3(0f, 3f, -20f);
        follow.smoothTime = 0.15f;

        cameraObject.transform.position = player.position + follow.offset;
        return camera;
    }

    private static GameUI CreateUI()
    {
        GameObject canvasObject = new GameObject("Canvas", typeof(RectTransform));
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();

        Font font = GetDefaultFont();

        GameObject coinTextObject = CreateUIObject("CoinText", canvasObject.transform);
        RectTransform coinRect = coinTextObject.GetComponent<RectTransform>();
        coinRect.anchorMin = new Vector2(0f, 1f);
        coinRect.anchorMax = new Vector2(0f, 1f);
        coinRect.pivot = new Vector2(0f, 1f);
        coinRect.anchoredPosition = new Vector2(30f, -30f);
        coinRect.sizeDelta = new Vector2(500f, 70f);

        Text coinText = coinTextObject.AddComponent<Text>();
        ConfigureText(coinText, font, 34, TextAnchor.UpperLeft, Color.white, "MOEDAS: 0");

        Shadow coinShadow = coinTextObject.AddComponent<Shadow>();
        coinShadow.effectColor = new Color(0f, 0f, 0f, 0.8f);
        coinShadow.effectDistance = new Vector2(2f, -2f);

        GameObject endPanel = new GameObject("EndPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        endPanel.transform.SetParent(canvasObject.transform, false);

        RectTransform panelRect = endPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(760f, 320f);

        Image panelImage = endPanel.GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.72f);

        GameObject messageObject = CreateUIObject("EndMessage", endPanel.transform);
        RectTransform messageRect = messageObject.GetComponent<RectTransform>();
        messageRect.anchorMin = new Vector2(0.5f, 0.5f);
        messageRect.anchorMax = new Vector2(0.5f, 0.5f);
        messageRect.pivot = new Vector2(0.5f, 0.5f);
        messageRect.anchoredPosition = new Vector2(0f, 55f);
        messageRect.sizeDelta = new Vector2(720f, 180f);

        Text endMessage = messageObject.AddComponent<Text>();
        ConfigureText(endMessage, font, 44, TextAnchor.MiddleCenter, Color.white, "FASE CONCLUÍDA!");

        GameObject buttonObject = new GameObject("RestartButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(endPanel.transform, false);

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = new Vector2(0f, -95f);
        buttonRect.sizeDelta = new Vector2(280f, 70f);

        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(0.15f, 0.55f, 0.95f, 1f);

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = buttonImage;

        GameObject buttonLabelObject = CreateUIObject("Label", buttonObject.transform);
        RectTransform labelRect = buttonLabelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text label = buttonLabelObject.AddComponent<Text>();
        ConfigureText(label, font, 30, TextAnchor.MiddleCenter, Color.white, "REINICIAR");

        GameUI gameUI = canvasObject.AddComponent<GameUI>();
        gameUI.coinText = coinText;
        gameUI.endPanel = endPanel;
        gameUI.endMessage = endMessage;
        gameUI.restartButton = button;

        endPanel.SetActive(false);
        return gameUI;
    }

    private static GameObject CreateUIObject(string objectName, Transform parent)
    {
        GameObject uiObject = new GameObject(objectName, typeof(RectTransform));
        uiObject.transform.SetParent(parent, false);
        return uiObject;
    }

    private static void ConfigureText(Text text, Font font, int fontSize, TextAnchor alignment, Color color, string content)
    {
        text.font = font;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = color;
        text.text = content;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
    }

    private static Font GetDefaultFont()
    {
        Font font = null;

        try
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
        catch
        {
            // Versoes anteriores usam Arial.ttf como fonte embutida.
        }

        if (font == null)
        {
            try
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            catch
            {
                // Fallback para uma fonte instalada no sistema operacional.
            }
        }

        if (font == null)
        {
            font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        }

        return font;
    }
}
