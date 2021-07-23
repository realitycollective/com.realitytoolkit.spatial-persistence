// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEditor;
using XRTK.Definitions.SpatialPersistence;
using XRTK.Services;

namespace XRTK.Editor.Profiles.TeleportSystem
{
    /// <summary>
    /// Reserved for future use as more providers are added.
    /// </summary>
    [CustomEditor(typeof(SpatialPersistenceSystemProfile))]
    public class MixedRealitySpatialPersistenceSystemProfileInspector : MixedRealityServiceProfileInspector
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
                MixedRealityToolkit.IsInitialized)
            {
                EditorApplication.delayCall += () => MixedRealityToolkit.Instance.ResetProfile(MixedRealityToolkit.Instance.ActiveProfile);
            }

            base.OnInspectorGUI();
        }
    }
}
