// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Definitions.SpatialPersistence;
using RealityToolkit.Interfaces.SpatialPersistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RealityToolkit.Services.SpatialPersistence
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
        {
        }

        #region IMixedRealitySpatialPersistenceSystem Implementation

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
            provider.StartSpatialPersistenceProvider();
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
                    provider.CreateAnchorSucceeded += OnCreateAnchorSucceeded;
                    provider.CreateAnchorFailed += OnCreateAnchorFailed;
                    provider.SpatialPersistenceStatusMessage += OnSpatialPersistenceStatusMessage;
                    provider.AnchorUpdated += OnAnchorUpdated;
                    provider.AnchorLocated += OnAnchorLocated;
                    provider.AnchorLocatedError += OnAnchorLocatedError;
                    provider.SpatialPersistenceError += OnSpatialPersistenceError;
                }
                else
                {
                    provider.CreateAnchorSucceeded -= OnCreateAnchorSucceeded;
                    provider.CreateAnchorFailed -= OnCreateAnchorFailed;
                    provider.SpatialPersistenceStatusMessage -= OnSpatialPersistenceStatusMessage;
                    provider.AnchorUpdated -= OnAnchorUpdated;
                    provider.AnchorLocated -= OnAnchorLocated;
                    provider.AnchorLocatedError -= OnAnchorLocatedError;
                    provider.SpatialPersistenceError -= OnSpatialPersistenceError;
                }
            }
        }

        /// <inheritdoc />
        public void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (var persistenceDataProvider in activeDataProviders)
            {
                persistenceDataProvider.TryCreateAnchor(position, rotation, timeToLive);
            }
        }

        /// <inheritdoc />
        public async Task<Guid> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (var persistenceDataProvider in activeDataProviders)
            {
                return await persistenceDataProvider.TryCreateAnchorAsync(position, rotation, timeToLive);
            }

            return Guid.Empty;
        }

        /// <inheritdoc />
        public void TryFindAnchorPoints(params Guid[] ids)
        {
            Debug.Assert(ids != null, "ID array is null");
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                persistenceDataProvider.TryFindAnchorPoints(ids);
            }
        }

        /// <inheritdoc />
        public async Task<bool> TryFindAnchorPointsAsync(params Guid[] ids)
        {
            Debug.Assert(ids != null, "ID array is null");
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                return await persistenceDataProvider.TryFindAnchorPointsAsync(ids);
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryMoveSpatialPersistence(GameObject anchoredObject, Vector3 worldPos, Quaternion worldRot, Guid cloudAnchorID)
        {
            Debug.Assert(anchoredObject != null, "Currently Anchored GameObject reference required");

            foreach (var persistenceDataProvider in activeDataProviders)
            {
                if (persistenceDataProvider.TryMoveSpatialPersistence(anchoredObject, worldPos, worldRot, cloudAnchorID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public void TryDeleteAnchors(params Guid[] ids)
        {
            foreach (var persistenceDataProvider in activeDataProviders)
            {
                persistenceDataProvider.DeleteAnchors(ids);
            }
        }

        /// <inheritdoc />
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
        private void OnCreateAnchorFailed() => CreateAnchorFailed?.Invoke();

        /// <inheritdoc />
        public event Action<Guid, GameObject> CreateAnchorSucceeded;
        private void OnCreateAnchorSucceeded(Guid id, GameObject anchoredObject) => CreateAnchorSucceeded?.Invoke(id, anchoredObject);

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceStatusMessage;
        private void OnSpatialPersistenceStatusMessage(string message) => SpatialPersistenceStatusMessage?.Invoke(message);

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceError;
        private void OnSpatialPersistenceError(string exception) => SpatialPersistenceError?.Invoke(exception);

        /// <inheritdoc />
        public event Action<Guid, GameObject> AnchorUpdated;
        private void OnAnchorUpdated(Guid id, GameObject gameObject) => AnchorUpdated?.Invoke(id, gameObject);

        /// <inheritdoc />
        public event Action<Guid, GameObject> AnchorLocated;
        private void OnAnchorLocated(Guid id, GameObject anchoredGameObject) => AnchorLocated?.Invoke(id, anchoredGameObject);

        /// <inheritdoc />
        public event Action<Guid, string> AnchorLocatedError;
        private void OnAnchorLocatedError(Guid id, string exception) => AnchorLocatedError?.Invoke(id, exception);

        #endregion IMixedRealitySpatialPersistenceSystem Implementation
    }
}
