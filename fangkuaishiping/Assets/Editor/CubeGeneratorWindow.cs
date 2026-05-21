using UnityEngine;
using UnityEditor;

public class CubeGeneratorWindow : EditorWindow
{
    private GameObject cubePrefab;
    // 间隔固定为 0，保证紧密排列
    private float spacing = 0f;

    // 基础数量
    private int count75 = 75;
    private int count4500 = 4500;

    // 各堆的目标心跳数（用于计算缩放）
    private readonly long[] targetCounts = {
        75,                // 1分钟
        4500,              // 1小时
        108000,            // 1天
        39420000,          // 1年
        3153600000         // 1生
    };
    private readonly string[] labels = {
        "1 Minute (75)",
        "1 Hour (4,500)",
        "1 Day (108,000)",
        "1 Year (39,420,000)",
        "1 Life (3,153,600,000)"
    };

    [MenuItem("Tools/Cube Generator")]
    public static void ShowWindow()
    {
        GetWindow<CubeGeneratorWindow>("Cube Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("心跳数量可视化 - 5堆生成器", EditorStyles.boldLabel);

        cubePrefab = (GameObject)EditorGUILayout.ObjectField("正方体预制体", cubePrefab, typeof(GameObject), false);
        count75 = EditorGUILayout.IntField("1分钟方块数 (真实生成)", count75);
        count4500 = EditorGUILayout.IntField("1小时方块数 (真实生成)", count4500);

        EditorGUILayout.HelpBox(
            "预制体 Scale 建议设为 0.01（方块会非常小）。\n" +
            "前两堆真实生成对应数量，后三堆使用 4500 个方块 + 立方根缩放。\n" +
            "所有堆都紧密排列（spacing = 0）。",
            MessageType.Info);

        if (GUILayout.Button("生成 5 堆心跳立方体"))
        {
            if (cubePrefab == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入正方体预制体", "确定");
                return;
            }
            GenerateAllStacks();
        }
    }

    private void GenerateAllStacks()
    {
        Vector3 prefabScale = cubePrefab.transform.localScale;

        // 紧密排列：步长 = 实际尺寸
        float stepX = prefabScale.x + spacing;
        float stepY = prefabScale.y + spacing;
        float stepZ = prefabScale.z + spacing;

        // 1. 计算 75 个方块的排列尺寸（尽量接近正方体）
        int size75 = Mathf.Max(1, Mathf.FloorToInt(Mathf.Pow(count75, 1f / 3f)));
        // 2. 计算 4500 个方块的排列尺寸
        int size4500 = Mathf.Max(1, Mathf.FloorToInt(Mathf.Pow(count4500, 1f / 3f)));

        // 5 堆的世界位置，一字排开
        float stackSpacing = 10f;
        for (int i = 0; i < targetCounts.Length; i++)
        {
            // 前两堆真实生成，后三堆用 4500 方块 + 缩放
            int baseCount = (i == 0) ? count75 : count4500;
            int baseSize = (i == 0) ? size75 : size4500;

            long target = targetCounts[i];
            float scaleFactor = Mathf.Pow((float)target / baseCount, 1f / 3f);

            // 创建父物体
            GameObject stackParent = new GameObject(labels[i]);
            Undo.RegisterCreatedObjectUndo(stackParent, "Create Heartbeat Stack");
            stackParent.transform.position = new Vector3(i * stackSpacing, 0, 0);
            stackParent.transform.localScale = Vector3.one * scaleFactor;

            // 生成该堆的所有方块（紧密排列）
            CreateCubesInBox(stackParent, baseSize, baseSize, baseSize, baseCount, stepX, stepY, stepZ);
        }

        Selection.activeGameObject = null;
        int totalCubes = count75 + count4500 * 4; // 75 + 4500*4 = 18075
        Debug.Log($"5 堆生成完毕。总方块数: {totalCubes}");
    }

    private void CreateCubesInBox(GameObject parent, int lenX, int lenY, int lenZ, int total, float stepX, float stepY, float stepZ)
    {
        int generated = 0;
        for (int x = 0; x < lenX; x++)
        {
            for (int y = 0; y < lenY; y++)
            {
                for (int z = 0; z < lenZ; z++)
                {
                    if (generated >= total) return;
                    Vector3 pos = new Vector3(x * stepX, y * stepY, z * stepZ);
                    CreateSingleCube(parent, pos);
                    generated++;
                }
            }
        }

        // 余数在 +X 方向继续放置
        int slice = 0;
        while (generated < total)
        {
            for (int y = 0; y < lenY; y++)
            {
                for (int z = 0; z < lenZ; z++)
                {
                    if (generated >= total) return;
                    Vector3 pos = new Vector3((lenX + slice) * stepX, y * stepY, z * stepZ);
                    CreateSingleCube(parent, pos);
                    generated++;
                }
            }
            slice++;
        }
    }

    private void CreateSingleCube(GameObject parent, Vector3 localPos)
    {
        GameObject cube = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab);
        cube.transform.parent = parent.transform;
        cube.transform.localPosition = localPos;
        cube.name = $"Cube_{localPos.x:F2}_{localPos.y:F2}_{localPos.z:F2}";
        Undo.RegisterCreatedObjectUndo(cube, "Create Heartbeat Cube");
    }
}