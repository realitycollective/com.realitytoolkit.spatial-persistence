// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityToolkit.SpatialPersistence.Interfaces;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence.Definitions
{
    public class SpatialPersistenceServiceProfile : BaseServiceProfile<ISpatialPersistenceServiceModule>
    {
        [Header("Settings")]
        [Tooltip("Should the service start automatically or manually when required (from a second scene).")]
        public AutoStartBehavior autoStartBehavior;
    }
}