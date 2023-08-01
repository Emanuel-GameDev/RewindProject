using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private SerializedProperty waitForStand;
    private SerializedProperty triggerOffset;
    private SerializedProperty platformTrigger;

    private bool showWaitForStandData = true;

    private void OnEnable()
    {
        waitForStand = serializedObject.FindProperty("waitForStand");
        triggerOffset = serializedObject.FindProperty("triggerOffset");
        platformTrigger = serializedObject.FindProperty("platformTrigger");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(waitForStand);

        if (waitForStand.boolValue)
        {
            showWaitForStandData = EditorGUILayout.BeginFoldoutHeaderGroup(showWaitForStandData, "WAIT FOR STAND DATA");
            if (showWaitForStandData)
            {
                EditorGUILayout.PropertyField(triggerOffset);
                EditorGUILayout.PropertyField(platformTrigger);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        // Mostra le altre variabili

        // LOOP
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("LOOP");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("loopPath"));

        // GENERAL
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("GENERAL");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("waypointPath"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));

        serializedObject.ApplyModifiedProperties();
    }
}

