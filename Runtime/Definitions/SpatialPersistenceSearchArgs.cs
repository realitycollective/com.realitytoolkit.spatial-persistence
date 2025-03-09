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

        public readonly bool IsValid =>
                // Check if the anchor ID is valid for cloud Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.CloudAnchor && !string.IsNullOrEmpty(anchorID)) ||
                // Check if the tracking texture is valid for image Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.ImageTracking && texture != null) ||
                // Check if the anchor ID is valid for global Anchors
                (spatialPersistenceTrackingType == SpatialPersistenceTrackingType.Any && !string.IsNullOrEmpty(anchorID)) ||
                spatialPersistenceTrackingType == SpatialPersistenceTrackingType.NotSupported;

        public SpatialPersistenceSearchArgs(SpatialPersistenceTrackingType spatialPersistenceTrackingType, string anchorID, Texture2D texture = null, string url = "")
        {
            this.spatialPersistenceTrackingType = spatialPersistenceTrackingType;
            this.anchorID = anchorID;
            this.texture = texture;
            this.url = url;
        }
    }
}