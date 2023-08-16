// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence.Definitions
{
    /// <summary>
    /// Tracked Image Data Construct
    /// </summary>    
    [Serializable]
    public class TrackedImageData
    {
        [Header("Trackable Configuration")]
        [SerializeField, Tooltip("The name for this image.")]
        private string name;

        public string Name { get => name; set => name = value; }

        [SerializeField, Tooltip("The source texture for the image. Must be marked as readable.")]
        private Texture2D texture;

        public Texture2D Texture { get => texture; set => texture = value; }

        [SerializeField, Tooltip("The actual physical size for the image. Measurement in meters.")]
        private float physicalSize = 1f;

        public float PhysicalSize { get => physicalSize; set => physicalSize = value; }

        [SerializeField, Tooltip("The url for this image.")]
        private string url;

        public string Url { get => url; set => url = value; }

        [SerializeField, Tooltip("The source guid for this image, used by the calling provider.")]
        private Guid sourceGuid;

        public Guid SourceGuid { get => sourceGuid; set => sourceGuid = value; }

        [SerializeField, Tooltip("The tracked guid for this image.")]
        private Guid referenceGuid;

        public Guid ReferenceGuid { get => referenceGuid; set => referenceGuid = value; }

        [Header("Settings")]
        [SerializeField, Tooltip("Is the content located locally or remote?")]
        private bool isLocal = false;

        public bool IsLocal { get => isLocal; set => isLocal = value; }

        /// <summary>
        /// The type of Tracked Object definition
        /// </summary>
        /// <remarks>
        /// Has URL, Not Local = Remote (Image downloaded and loaded)
        /// Has URL, IS Local = Local (image loaded)
        /// No URL, Has Texture = Local (image loaded)
        /// No URL, No Texture = Builtin (No Processsing, handled by ARImageTrackedHandler)
        /// </remarks>
        public ImageDataType ImageDataType => !string.IsNullOrEmpty(url) ?
            // Has URL
            IsLocal ?
                // Local flag set
                ImageDataType.BuiltIn :
                // Local flag not set
                ImageDataType.Remote :
            // No URL
            Texture != null ?
                // Has texture
                ImageDataType.Local :
                // No texture
                ImageDataType.BuiltIn;

    }
}