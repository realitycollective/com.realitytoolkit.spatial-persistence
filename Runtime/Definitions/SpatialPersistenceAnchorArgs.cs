// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence
{
    public struct SpatialPersistenceAnchorArgs
    {
        public Guid guid;
        public Texture2D texture;
        public string url;

        public SpatialPersistenceAnchorArgs(Guid guid, Texture2D texture = null, string url = "")
        {
            this.guid = guid;
            this.texture = texture;
            this.url = url;
        }
    }
}