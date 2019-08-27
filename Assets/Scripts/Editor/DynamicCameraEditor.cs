using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicCamera))]
public class DynamicCameraEditor : Editor
{
    private float labelWidth = 150f;
    private float buttonWidth = 100f;
    private Feature current;

    public override void OnInspectorGUI()
	{
        DynamicCamera main = (DynamicCamera)target;

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

        if (GUILayout.Button("Preview Base", GUILayout.Width(buttonWidth)))
        {
            main.PositionCamera(true);
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

            if (GUILayout.Button("Preview", GUILayout.Width(buttonWidth)))
                main.PositionCamera(true, current.id);

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
                EditorGUILayout.Space();

                current.id = EditorGUILayout.TextField("ID", current.id);
                current.target = (Transform) EditorGUILayout.ObjectField("Target", current.target, typeof(Transform), true);
                current.offset = EditorGUILayout.Vector3Field("Offset", current.offset);
                current.angle = EditorGUILayout.FloatField("Angle", current.angle);

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
    }
}
