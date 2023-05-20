using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(CompositeBehavior))]
public class CompositeBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CompositeBehavior compositeBehavior = (CompositeBehavior)target;

        Rect rect = EditorGUILayout.BeginHorizontal();
        rect.height = EditorGUIUtility.singleLineHeight;

        //check for behavior
        if (compositeBehavior.behaviors == null || compositeBehavior.behaviors.Length == 0)
        {
            EditorGUILayout.HelpBox("No behaviors in array", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
            rect = EditorGUILayout.BeginHorizontal();
            rect.height= EditorGUIUtility.singleLineHeight;
        }
        else
        {
            rect.x = 30f;
            rect.width = EditorGUIUtility.currentViewWidth - 95f;
            EditorGUI.LabelField(rect, "Behaviors");
            rect.x = EditorGUIUtility.currentViewWidth - 65f;
            rect.width = 60f;
            EditorGUI.LabelField(rect, "Weight");
            rect.y += EditorGUIUtility.singleLineHeight * 1.2f;

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < compositeBehavior.behaviors.Length; i++)
            {
                rect.x = 5f;
                rect.width = 20f;
                EditorGUI.LabelField(rect, i.ToString());
                rect.x = 30f;
                rect.width = EditorGUIUtility.currentViewWidth - 95f;
                compositeBehavior.behaviors[i]
                    = (FlockBehavior)EditorGUI.ObjectField(rect, compositeBehavior.behaviors[i], typeof(FlockBehavior), false);
                rect.x = EditorGUIUtility.currentViewWidth - 65f;
                rect.width = 60f;
                compositeBehavior.weights[i] = EditorGUI.FloatField(rect, compositeBehavior.weights[i]);
                rect.y += EditorGUIUtility.singleLineHeight * 1.2f;
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(compositeBehavior);
            }
        }

        EditorGUILayout.EndHorizontal();
        rect.x = 5f;
        rect.width = EditorGUIUtility.currentViewWidth - 10f;
        rect.y += EditorGUIUtility.singleLineHeight * 1.5f;
        if (GUI.Button(rect, "Add Behavior"))
        {
            AddBehavior(compositeBehavior);
            EditorUtility.SetDirty(compositeBehavior);
        }

        rect.y += EditorGUIUtility.singleLineHeight * 1.5f;

        if (compositeBehavior.behaviors.Length > 0 && compositeBehavior.behaviors != null)
        {
            if (GUI.Button(rect, "Remove Behavior"))
            {
                RemoveBehavior(compositeBehavior);
                EditorUtility.SetDirty(compositeBehavior);
            }
        }

    }

    void AddBehavior(CompositeBehavior compositeBehavior)
    {
        int count = (compositeBehavior.behaviors == null) ? 0 : compositeBehavior.behaviors.Length;
        FlockBehavior[] newBehaviors = new FlockBehavior[count + 1];
        float[] newWeights = new float[count + 1];
        for (int i = 0; i < count; i++)
        {
            newBehaviors[i] = compositeBehavior.behaviors[i];
            newWeights[i] = compositeBehavior.weights[i];
        }

        newWeights[count] = 1f;
        compositeBehavior.behaviors = newBehaviors;
        compositeBehavior.weights = newWeights;
    }

    void RemoveBehavior(CompositeBehavior compositeBehavior)
    {
        int count = compositeBehavior.behaviors.Length;

        if (count == 1)
        {
            compositeBehavior.behaviors = null;
            compositeBehavior.weights = null;
            return;
        }

        FlockBehavior[] newBehaviors = new FlockBehavior[count - 1];
        float[] newWeights = new float[count - 1];
        for (int i = 0; i < count - 1; i++)
        {
            newBehaviors[i] = compositeBehavior.behaviors[i];
            newWeights[i] = compositeBehavior.weights[i];
        }

        compositeBehavior.behaviors = newBehaviors;
        compositeBehavior.weights = newWeights;
    }
}