// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Editor.Profiles;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.SpatialPersistence.Definitions;
using UnityEditor;

namespace RealityToolkit.SpatialPersistence.Editor
{
    /// <summary>
    /// Reserved for future use as more providers are added.
    /// </summary>
    [CustomEditor(typeof(SpatialPersistenceSystemProfile))]
    public class SpatialPersistenceSystemProfileInspector : ServiceProfileInspector
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            RenderHeader("The anchor system profile defines behaviour for the anchor system.");

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck() &&
                ServiceManager.Instance != null &&
                ServiceManager.Instance.IsInitialized)
            {
                EditorApplication.delayCall += () => ServiceManager.Instance.ResetProfile(ServiceManager.Instance.ActiveProfile);
            }

            base.OnInspectorGUI();
        }
    }
}
