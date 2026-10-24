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
        Any,
        CloudAnchor = 1,
        ImageTracking = 2,

        /// <summary>
        /// Anchors localised against a map that was scanned and is stored on the device, with no cloud service involved
        /// (for example Niantic device mapping). The anchor ID identifies the stored map.
        /// </summary>
        DeviceMap = 3,

        /// <summary>
        /// Anchors at a geographic position, found by GPS and visual positioning. A search names the position in
        /// <see cref="SpatialPersistenceSearchArgs.coordinate"/>.
        /// </summary>
        Geospatial = 4
    }
}