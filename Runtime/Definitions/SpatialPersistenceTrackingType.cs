// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace RealityToolkit.SpatialPersistence
{
    /// <summary>
    /// How does the Spatial Persistence module locate its anchors?
    /// </summary>
    public enum SpatialPersistenceTrackingType
    {
        NotSupported = 0,
        CloudAnchor = 1,
        ImageTracking = 2
    }
}