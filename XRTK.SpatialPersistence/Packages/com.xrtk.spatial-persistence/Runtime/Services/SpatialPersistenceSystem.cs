// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using UnityEngine;
using XRTK.Definitions.SpatialPersistence;
using XRTK.Interfaces.SpatialPersistence;

namespace XRTK.Services.SpatialPersistence
{
    /// <summary>
    /// Concrete implementation of the <see cref="IMixedRealitySpatialPersistenceSystem"/>
    /// </summary>
    [System.Runtime.InteropServices.Guid("C055102F-5204-42ED-A4D8-F80D129B6BBD")]
    public class SpatialPersistenceSystem : BaseSystem, IMixedRealitySpatialPersistenceSystem
    {
        /// <inheritdoc />
        public SpatialPersistenceSystem(SpatialPersistenceSystemProfile profile) 
            : base(profile)
        { }

        #region IMixedRealitySpatialPersistenceSystem Implementation

        public void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            Debug.Assert(timeToLive == new DateTimeOffset(), "Lifetime of SpatialPersistence required");

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                persistenceDataProvider.TryCreateAnchor(position, rotation, timeToLive);
            }
        }

        public bool TryFindAnchorPoints(params Guid[] ids)
        {
            Debug.Assert(ids != null, "ID array is null");
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                if (persistenceDataProvider.TryFindAnchorPoints(ids))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryMoveSpatialPersistence(GameObject anchoredObject, Vector3 worldPos, Quaternion worldRot, Guid cloudAnchorID = new Guid())
        {
            Debug.Assert(anchoredObject != null, "Currently SpatialPersistenceed GameObject reference required");

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                if (persistenceDataProvider.TryMoveSpatialPersistence(anchoredObject, worldPos, worldRot, cloudAnchorID))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryClearAnchorCache()
        {
            var anyClear = false;

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                if (persistenceDataProvider.TryClearAnchorCache())
                {
                    anyClear = true;
                }
            }

            return anyClear;
        }

        /// <inheritdoc />
        public event Action CreateAnchorFailed;

        /// <inheritdoc />
        public event Action<Guid, GameObject> CreateAnchorSucceeded;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceStatusMessage;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceError;

        /// <inheritdoc />
        public event Action<Guid, GameObject> AnchorUpdated;

        /// <inheritdoc />
        public event Action<Guid, GameObject> AnchorLocated;

        #endregion IMixedRealitySpatialPersistenceSystem Implementation

        #region BaseSystem Implementation

        private readonly HashSet<IMixedRealitySpatialPersistenceDataProvider> activeDataProviders = new HashSet<IMixedRealitySpatialPersistenceDataProvider>();

        /// <inheritdoc />
        public IReadOnlyCollection<IMixedRealitySpatialPersistenceDataProvider> ActiveSpatialPersistenceProviders => activeDataProviders;

        /// <inheritdoc />
        public bool RegisterSpatialPersistenceDataProvider(IMixedRealitySpatialPersistenceDataProvider provider)
        {
            if (activeDataProviders.Contains(provider))
            {
                return false;
            }

            activeDataProviders.Add(provider);
            SpatialPersistenceEvents(provider, true);

            return true;
        }

        /// <inheritdoc />
        public bool UnRegisterSpatialPersistenceDataProvider(IMixedRealitySpatialPersistenceDataProvider provider)
        {
            if (!activeDataProviders.Contains(provider))
            {
                return false;
            }

            SpatialPersistenceEvents(provider, false);
            activeDataProviders.Remove(provider);

            return true;
        }

        private void SpatialPersistenceEvents(IMixedRealitySpatialPersistenceDataProvider provider, bool isRegistered)
        {
            if (activeDataProviders != null && activeDataProviders.Contains(provider))
            {
                if (isRegistered)
                {
                    provider.CreateAnchorSucceeded += CreateAnchorSucceeded;
                    provider.CreateAnchorFailed += CreateAnchorFailed;
                    provider.SpatialPersistenceStatusMessage += SpatialPersistenceStatusMessage;
                    provider.AnchorUpdated += AnchorUpdated;
                    provider.AnchorLocated += AnchorLocated;
                    provider.SpatialPersistenceError += SpatialPersistenceError;
                }
                else
                {
                    provider.CreateAnchorSucceeded -= CreateAnchorSucceeded;
                    provider.CreateAnchorFailed -= CreateAnchorFailed;
                    provider.SpatialPersistenceStatusMessage -= SpatialPersistenceStatusMessage;
                    provider.AnchorUpdated -= AnchorUpdated;
                    provider.AnchorLocated -= AnchorLocated;
                    provider.SpatialPersistenceError -= SpatialPersistenceError;
                }
            }
        }

        #endregion BaseSystem Implementation
    }
}
