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
    public class SpatialPersistenceSystem : BaseExtensionService, IMixedRealitySpatialPersistenceSystem
    {
        private IMixedRealitySpatialPersistenceDataProvider currentSpatialPersistenceProvider;

        [HideInInspector]
        public IMixedRealitySpatialPersistenceDataProvider CurrentSpatialPersistenceProvider => currentSpatialPersistenceProvider;

        /// <inheritdoc />
        public SpatialPersistenceSystem(string name, uint priority, SpatialPersistenceSystemProfile profile) : base(name, priority, profile)
        { }

        #region IMixedRealitySpatialPersistenceSystem Implementation

        public void CreateAnchoredObject(GameObject objectToSpatialPersistencePrefab, Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            Debug.Assert(objectToSpatialPersistencePrefab, "Prefab Missing");
            Debug.Assert(timeToLive == new DateTimeOffset(), "Lifetime of SpatialPersistence required");

            currentSpatialPersistenceProvider.CreateAnchoredObject(objectToSpatialPersistencePrefab, position, rotation, timeToLive);
        }

        public bool FindAnchorPoint(string id)
        {
            Debug.Assert(string.IsNullOrEmpty(id), "ID required for SpatialPersistence search");

            return currentSpatialPersistenceProvider.FindAnchorPoints(new[] { id });
        }

        public bool FindAnchorPoints(string[] ids)
        {
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            return currentSpatialPersistenceProvider.FindAnchorPoints(ids);
        }

        public bool PlaceSpatialPersistence(string id, GameObject objectToPlace)
        {
            Debug.Assert(!string.IsNullOrEmpty(id), "SpatialPersistence ID is null");
            Debug.Assert(objectToPlace != null, "Object To SpatialPersistence Prefab is null");

            return currentSpatialPersistenceProvider.PlaceAnchoredObject(id, objectToPlace);
        }

        public bool MoveSpatialPersistence(GameObject anchoredObject, Vector3 worldPos, Quaternion worldRot, string cloudSpatialPersistenceID = "")
        {
            Debug.Assert(anchoredObject != null, "Currently SpatialPersistenceed GameObject reference required");

            return currentSpatialPersistenceProvider.MoveAnchoredObject(anchoredObject, worldPos, worldRot, cloudSpatialPersistenceID);
        }

        public bool TryClearAnchors() => currentSpatialPersistenceProvider.TryClearAnchors();

        /// <inheritdoc />
        public event Action CreateAnchoredObjectFailed;

        /// <inheritdoc />
        public event Action<string, GameObject> CreateAnchoredObjectSucceeded;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceStatusMessage;

        /// <inheritdoc />
        public event Action<string, GameObject> CloudAnchorUpdated;

        /// <inheritdoc />
        public event Action<string> CloudAnchorLocated;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceError;
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
            currentSpatialPersistenceProvider = provider;   

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
            currentSpatialPersistenceProvider = null;

            return true;
        }

        private void SpatialPersistenceEvents(IMixedRealitySpatialPersistenceDataProvider provider, bool isRegistered)
        {
            if (activeDataProviders != null && activeDataProviders.Contains(provider))
            {
                if (isRegistered)
                {
                    provider.CreateAnchoredObjectSucceeded += CreateAnchoredObjectSucceeded;
                    provider.CreateAnchoredObjectFailed += CreateAnchoredObjectFailed;
                    provider.SpatialPersistenceStatusMessage += SpatialPersistenceStatusMessage;
                    provider.CloudAnchorUpdated += CloudAnchorUpdated;
                    provider.CloudAnchorLocated += CloudAnchorLocated;
                    provider.SpatialPersistenceError += SpatialPersistenceError;
                }
                else
                {
                    provider.CreateAnchoredObjectSucceeded -= CreateAnchoredObjectSucceeded;
                    provider.CreateAnchoredObjectFailed -= CreateAnchoredObjectFailed;
                    provider.SpatialPersistenceStatusMessage -= SpatialPersistenceStatusMessage;
                    provider.CloudAnchorUpdated -= CloudAnchorUpdated;
                    provider.CloudAnchorLocated -= CloudAnchorLocated;
                    provider.SpatialPersistenceError -= SpatialPersistenceError;
                }
            }
        }

        #endregion BaseSystem Implementation
    }
}
