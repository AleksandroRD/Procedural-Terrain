using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainDemo))]
public class TerrainDemoEditor : Editor
{
    
    public override void OnInspectorGUI(){
        TerrainDemo terrainDemo = (TerrainDemo)target;
        
        DrawDefaultInspector();
        
        if (GUILayout.Button ("Generate")) {
		    terrainDemo.GenerateSimpleTerrain();
		}
    }
}
