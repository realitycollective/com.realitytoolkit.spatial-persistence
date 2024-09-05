// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Editor;
using RealityCollective.ServiceFramework.Editor.Packages;
using RealityCollective.Utilities.Editor;
using RealityCollective.Utilities.Extensions;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence.Editor
{
    [InitializeOnLoad]
    internal static class SpatialPersistencePackageInstaller
    {
        private static readonly string destinationPath = Application.dataPath + "/RealityToolkit/SpatialPersistence";
        private static readonly string sourcePath = Path.GetFullPath($"{PathFinderUtility.ResolvePath<IPathFinder>(typeof(SpatialPersistencePackagePathFinder)).ForwardSlashes()}{Path.DirectorySeparatorChar}{"Assets~"}");

        static SpatialPersistencePackageInstaller()
        {
            EditorApplication.delayCall += CheckPackage;
        }

        [MenuItem(ServiceFrameworkPreferences.Editor_Menu_Keyword + "/Reality Toolkit/Packages/Install Spatial Persistence Package Assets...", true)]
        private static bool ImportPackageAssetsValidation()
        {
            return !Directory.Exists($"{destinationPath}{Path.DirectorySeparatorChar}");
        }

        [MenuItem(ServiceFrameworkPreferences.Editor_Menu_Keyword + "/Reality Toolkit/Packages/Install Spatial Persistence Package Assets...")]
        private static void ImportPackageAssets()
        {
            EditorPreferences.Set($"{nameof(SpatialPersistencePackageInstaller)}.Assets", false);
            EditorApplication.delayCall += CheckPackage;
        }

        private static void CheckPackage()
        {
            if (!EditorPreferences.Get($"{nameof(SpatialPersistencePackageInstaller)}.Assets", false))
            {
                EditorPreferences.Set($"{nameof(SpatialPersistencePackageInstaller)}.Assets", AssetsInstaller.TryInstallAssets(sourcePath, destinationPath));
            }
        }
    }
}