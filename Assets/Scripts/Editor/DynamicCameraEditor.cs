using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicCamera))]
public class DynamicCameraEditor : Editor
{
    private float labelWidth = 150f;
    private float buttonWidth = 100f;
    private Feature current;
    private DynamicCamera main;

    public override void OnInspectorGUI()
	{
        main = (DynamicCamera)target;

        Undo.RecordObject(main, "DynamicCamera");

        EditorGUILayout.Space();

        // Settings
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        main.smoothTime = EditorGUILayout.Slider("Smooth Time", main.smoothTime, 0.05f, 2f);
        main.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", main.rotationSpeed);
        main.zoomDampening = EditorGUILayout.FloatField("Zoom Dampening", main.zoomDampening);

        EditorGUILayout.Space();

        // BaseObject
        EditorGUILayout.LabelField("Base Target", EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        main.target = (Transform) EditorGUILayout.ObjectField("Base Target", main.target, typeof(Transform));
        main.offset = EditorGUILayout.Vector3Field("Base Target Offset", main.offset);
        main.angle = EditorGUILayout.FloatField("Base Target Angle", main.angle);

        if (GUI.changed && current == null)
        {
            CollapseAll();
            SetPosition();
        }

        if (GUILayout.Button("Preview Base", GUILayout.Width(buttonWidth)))
        {
            SetPosition();
        }

        EditorGUILayout.Space();

        // Parts
        EditorGUILayout.LabelField("Parts", EditorStyles.boldLabel, GUILayout.Width(labelWidth));


        // Loops through the list
        for (int i = 0; i < main.features.Count; i++)
        {
            // cache current
            current = main.features[i];

            EditorGUILayout.BeginHorizontal();

            current.toggleFold = EditorGUILayout.Foldout(current.toggleFold, current.id);

            // Preview Button
            if (GUILayout.Button("Preview", GUILayout.Width(buttonWidth)))
            {
                SetPosition(current.id);
            }

            // if clicked, remove current from the list
            if (GUILayout.Button("Remove", GUILayout.Width(buttonWidth)))
            {
                main.features.Remove(current);
                break;
            }

            EditorGUILayout.EndHorizontal();

            // If the class is expanded
            if (current.toggleFold)
            {
                for (int j = 0; j < main.features.Count; j++)
                {
                    if (main.features[j] != current)
                        main.features[j].toggleFold = false;
                }

                EditorGUILayout.Space();

                current.id = EditorGUILayout.TextField("ID", current.id);
                current.target = (Transform) EditorGUILayout.ObjectField("Target", current.target, typeof(Transform), true);
                current.offset = EditorGUILayout.Vector3Field("Offset", current.offset);
                current.angle = EditorGUILayout.FloatField("Angle", current.angle);

                if (GUI.changed)
                    SetPosition(current.id);

                EditorGUILayout.Space();
            }

            current = null;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add New", GUILayout.Width(buttonWidth)))
        {
            Feature temp = new Feature();
            main.features.Add(temp);
        }

        EditorUtility.SetDirty(main);
    }

    private void SetPosition(string id = "")
    {
        main.PositionCamera(true, id);
    }

    public void CollapseAll()
    {
        foreach (Feature f in main.features)
            f.toggleFold = false;
    }
}
