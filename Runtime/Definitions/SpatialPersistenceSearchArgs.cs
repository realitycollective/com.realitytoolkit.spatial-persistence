// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace RealityToolkit.SpatialPersistence
{
    public struct SpatialPersistenceSearchArgs
    {
        public SpatialPersistenceTrackingType spatialPersistenceTrackingType;
        public string anchorID;
        public Texture2D texture;
        public string url;

        /// <summary>
        /// The stored map to search, for <see cref="SpatialPersistenceTrackingType.DeviceMap"/>. A search may name the map instead of an anchor.
        /// </summary>
        public string mapPackageId;

        /// <summary>
        /// The position to search at, for <see cref="SpatialPersistenceTrackingType.Geospatial"/>.
        /// </summary>
        public SpatialPersistenceGeoCoordinate? coordinate;

        public readonly bool IsValid =>
                // Check if the anchor ID is valid for cloud Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.CloudAnchor && !string.IsNullOrEmpty(anchorID)) ||
                // Check if the tracking texture is valid for image Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.ImageTracking && texture != null) ||
                // Check if the anchor ID, which names the stored map, is valid for device map Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.DeviceMap && (!string.IsNullOrEmpty(anchorID) || !string.IsNullOrEmpty(mapPackageId))) ||
                // Check if the position is valid for geospatial Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.Geospatial && coordinate.HasValue && coordinate.Value.IsValid) ||
                // Check if the anchor ID is valid for global Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.Any && !string.IsNullOrEmpty(anchorID)) ||
                spatialPersistenceTrackingType == SpatialPersistenceTrackingType.NotSupported;

        public SpatialPersistenceSearchArgs(SpatialPersistenceTrackingType spatialPersistenceTrackingType, string anchorID, Texture2D texture = null, string url = "")
        {
            this.spatialPersistenceTrackingType = spatialPersistenceTrackingType;
            this.anchorID = anchorID;
            this.texture = texture;
            this.url = url;
            mapPackageId = null;
            coordinate = null;
        }

        /// <summary>
        /// A search for a stored device map, or for a geospatial position.
        /// </summary>
        public SpatialPersistenceSearchArgs(SpatialPersistenceTrackingType spatialPersistenceTrackingType, string anchorID, string mapPackageId, SpatialPersistenceGeoCoordinate? coordinate = null)
            : this(spatialPersistenceTrackingType, anchorID)
        {
            this.mapPackageId = mapPackageId;
            this.coordinate = coordinate;
        }
    }

    /// <summary>
    /// A WGS 84 position for a geospatial search. Kept small so the package needs no geographic library; apps convert at the boundary.
    /// </summary>
    public readonly struct SpatialPersistenceGeoCoordinate
    {
        public SpatialPersistenceGeoCoordinate(double latitude, double longitude, double altitude = 0d)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        /// <summary>
        /// Degrees north, -90 to 90.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Degrees east, -180 to 180.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Metres above the WGS 84 ellipsoid.
        /// </summary>
        public double Altitude { get; }

        /// <summary>
        /// True for a position on the globe.
        /// </summary>
        public bool IsValid => Latitude >= -90d && Latitude <= 90d && Longitude >= -180d && Longitude <= 180d && !(Latitude == 0d && Longitude == 0d);
    }
}