using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WorldGenerator worldGenerator = (WorldGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            worldGenerator.PreviewMap();

        }
    }
}
