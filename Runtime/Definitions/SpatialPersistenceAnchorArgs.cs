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

        public SpatialPersistenceAnchorArgs(Guid guid, Texture2D texture)
        {
            this.guid = guid;
            this.texture = texture;
        }
    }
}