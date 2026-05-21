using UnityEngine;
using UnityEditor;

public class CubeGeneratorWindow : EditorWindow
{
    // ---------- 预制体 & 间隔 ----------
    private GameObject cubePrefab;      // 正方体预制体
    private float spacing = 0f;         // 小正方体之间的间隔

    // ---------- 尺寸模式 ----------
    private enum SizeMode
    {
        Manual,     // 手动输入长宽高
        AutoTotal   // 自动根据总数计算
    }
    private SizeMode sizeMode = SizeMode.AutoTotal;

    // 手动模式
    private int manualLength = 1;
    private int manualWidth = 1;
    private int manualHeight = 1;

    // 自动总数模式
    private int totalCount = 1000;

    [MenuItem("Tools/Cube Generator")]
    public static void ShowWindow()
    {
        GetWindow<CubeGeneratorWindow>("Cube Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("大立方体生成器", EditorStyles.boldLabel);

        // 预制体
        cubePrefab = (GameObject)EditorGUILayout.ObjectField("正方体预制体", cubePrefab, typeof(GameObject), false);

        // 间隔
        spacing = EditorGUILayout.FloatField("间隔 (Spacing)", spacing);
        if (spacing < 0) spacing = 0;

        EditorGUILayout.Space(10);

        // 尺寸模式选择
        sizeMode = (SizeMode)EditorGUILayout.EnumPopup("尺寸模式", sizeMode);

        if (sizeMode == SizeMode.Manual)
        {
            manualLength = EditorGUILayout.IntField("长 (X轴数量)", manualLength);
            manualWidth = EditorGUILayout.IntField("宽 (Z轴数量)", manualWidth);
            manualHeight = EditorGUILayout.IntField("高 (Y轴数量)", manualHeight);

            if (manualLength < 1) manualLength = 1;
            if (manualWidth < 1) manualWidth = 1;
            if (manualHeight < 1) manualHeight = 1;
        }
        else // AutoTotal
        {
            totalCount = EditorGUILayout.IntField("总数", totalCount);
            if (totalCount < 0) totalCount = 0;
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("生成大立方体"))
        {
            GenerateCube();
        }
    }

    private void GenerateCube()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("请先指定一个正方体预制体！");
            return;
        }

        int length, width, height;
        int remainder = 0; // 余数

        if (sizeMode == SizeMode.Manual)
        {
            length = manualLength;
            width = manualWidth;
            height = manualHeight;
        }
        else
        {
            // 自动计算：取总数立方根向下取整，尽量接近正方体
            if (totalCount == 0)
            {
                Debug.LogWarning("总数为0，不生成任何物体。");
                return;
            }

            int baseSize = Mathf.Max(1, Mathf.FloorToInt(Mathf.Pow(totalCount, 1f / 3f)));
            length = baseSize;
            width = baseSize;
            height = baseSize;

            int baseTotal = length * width * height;
            remainder = totalCount - baseTotal;
        }

        // 创建父物体
        GameObject parentObject = new GameObject("BigCube");
        Undo.RegisterCreatedObjectUndo(parentObject, "Create BigCube");

        float step = 1f + spacing; // 相邻中心距离

        // ---------- 生成主体部分 (长 x 宽 x 高) ----------
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    CreateCubeAt(parentObject, x, y, z, step);
                }
            }
        }

        // ---------- 生成余数部分 ----------
        if (sizeMode == SizeMode.AutoTotal && remainder > 0)
        {
            // 余数排列规则：紧贴在主立方体的X轴正方向，
            // 每一层可以放 (width * height) 个，层满则继续向外扩展。
            int perSlice = width * height;

            for (int i = 0; i < remainder; i++)
            {
                int sliceIndex = i / perSlice;
                int indexInSlice = i % perSlice;

                int addX = length + sliceIndex;                 // X 在主立方体右边依次排列
                int addZ = indexInSlice % width;
                int addY = indexInSlice / width;

                CreateCubeAt(parentObject, addX, addY, addZ, step);
            }
        }

        // 选中最终生成物
        Selection.activeGameObject = parentObject;
        Debug.Log($"生成完成：主体 {length}x{width}x{height}，额外 {remainder} 个，总计 {length * width * height + remainder} 个。");
    }

    private void CreateCubeAt(GameObject parent, int x, int y, int z, float step)
    {
        GameObject cube = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab);
        cube.transform.parent = parent.transform;
        cube.transform.localPosition = new Vector3(x * step, y * step, z * step);
        cube.name = $"Cube_{x}_{y}_{z}";
        Undo.RegisterCreatedObjectUndo(cube, "Create BigCube");
    }
}