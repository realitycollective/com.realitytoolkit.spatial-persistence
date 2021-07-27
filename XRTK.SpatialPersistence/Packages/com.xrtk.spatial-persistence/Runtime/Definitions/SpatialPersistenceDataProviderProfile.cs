// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using XRTK.Definitions.Utilities;
using XRTK.Interfaces.SpatialPersistence;

namespace XRTK.Definitions.SpatialPersistence
{
    /// <summary>
    /// The configuration profile for <see cref="IMixedRealitySpatialPersistenceDataProvider"/>.
    /// </summary>
    [CreateAssetMenu(menuName = "Mixed Reality Toolkit/SpatialPersistences/SpatialPersistence Data Provider", fileName = "SpatialPersistenceDataProviderProfile", order = (int)CreateProfileMenuItemIndices.RegisteredServiceProviders)]
    public class SpatialPersistenceDataProviderProfile : BaseMixedRealityProfile
    { }
}
