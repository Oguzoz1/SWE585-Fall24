using Generator.Asteroid;
using PathFinding.Octrees;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidGenerator))]
public class AsteroidGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AsteroidGenerator generator = (AsteroidGenerator)target;
        if (GUILayout.Button("Generate Asteroids"))
        {
            generator.GenerateAsteroids();
        }
    }
}

[CustomEditor(typeof(OctreeGenerator))]
public class OctreeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        OctreeGenerator generator = (OctreeGenerator)target;
        if (GUILayout.Button("Initialize Octree"))
        {
            generator.InitializeOctree();
        }
    }
}