// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Definitions.Utilities;
using RealityCollective.ServiceFramework.Interfaces;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.SpatialPersistence.Definitions;
using RealityToolkit.SpatialPersistence.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence
{
    /// <summary>
    /// Concrete implementation of the <see cref="ISpatialPersistenceService"/>
    /// </summary>
    [System.Runtime.InteropServices.Guid("C055102F-5204-42ED-A4D8-F80D129B6BBD")]
    public class SpatialPersistenceService : BaseServiceWithConstructor, ISpatialPersistenceService
    {
        #region Private Properties
        private AutoStartBehavior autoStartBehavior = AutoStartBehavior.AutoStart;
        #endregion Private Properties

        #region Constructor
        /// <inheritdoc />
        public SpatialPersistenceService(string name, uint priority, SpatialPersistenceServiceProfile profile)
            : base(name, priority)
        {
            autoStartBehavior = profile.autoStartBehavior;
        }
        #endregion Constructor

        #region MonoBehaviours
        public override void Destroy()
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.StopSpatialPersistenceProvider();
                UnRegisterServiceModule(persistenceServiceModule);
            }
            base.Destroy();
        }
        #endregion MonoBehaviours

        #region IMixedRealitySpatialPersistenceSystem Implementation
        /// <inheritdoc />
        public async Task StartSpatialPersistenceService()
        {
            if (ServiceModules.Count > 0)
            {
                foreach (ISpatialPersistenceServiceModule spatialProvider in ServiceModules)
                {
                    await spatialProvider.StartSpatialPersistenceProvider();
                }
            }
        }

        /// <inheritdoc />
        public void StopSpatialPersistenceService()
        {
            if (ServiceModules.Count > 0)
            {
                foreach (ISpatialPersistenceServiceModule spatialProvider in ServiceModules)
                {
                    spatialProvider.StopSpatialPersistenceProvider();
                }
            }
        }

        /// <inheritdoc />
        public void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.TryCreateAnchor(position, rotation, timeToLive);
            }
        }

        /// <inheritdoc />
        public async Task<Guid> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (ISpatialPersistenceServiceModule persistenceDataProvider in ServiceModules)
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

            foreach (ISpatialPersistenceServiceModule persistenceDataProvider in ServiceModules)
            {
                persistenceDataProvider.TryFindAnchorPoints(ids);
            }
        }

        /// <inheritdoc />
        public async Task<bool> TryFindAnchorPointsAsync(params Guid[] ids)
        {
            Debug.Assert(ids != null, "ID array is null");
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            foreach (ISpatialPersistenceServiceModule persistenceDataProvider in ServiceModules)
            {
                return await persistenceDataProvider.TryFindAnchorPointsAsync(ids);
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryMoveSpatialPersistence(GameObject anchoredObject, Vector3 worldPos, Quaternion worldRot, Guid cloudAnchorID)
        {
            Debug.Assert(anchoredObject != null, "Currently Anchored GameObject reference required");

            foreach (ISpatialPersistenceServiceModule persistenceDataProvider in ServiceModules)
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
            foreach (ISpatialPersistenceServiceModule persistenceDataProvider in ServiceModules)
            {
                persistenceDataProvider.DeleteAnchors(ids);
            }
        }

        /// <inheritdoc />
        public bool TryClearAnchorCache()
        {
            var anyClear = false;

            foreach (ISpatialPersistenceServiceModule persistenceDataProvider in ServiceModules)
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

        #region BaseService Implementation
        /// <inheritdoc />
        public override void RegisterServiceModule(IServiceModule serviceModule)
        {
            base.RegisterServiceModule(serviceModule);

            SpatialPersistenceEvents(serviceModule as ISpatialPersistenceServiceModule, true);
            if (autoStartBehavior == AutoStartBehavior.AutoStart)
            {
                (serviceModule as ISpatialPersistenceServiceModule).StartSpatialPersistenceProvider();
            }
        }

        /// <inheritdoc />
        public override void UnRegisterServiceModule(IServiceModule serviceModule)
        {
            SpatialPersistenceEvents(serviceModule as ISpatialPersistenceServiceModule, false);
            (serviceModule as ISpatialPersistenceServiceModule).StopSpatialPersistenceProvider();
            base.UnRegisterServiceModule(serviceModule);
        }
        #endregion BaseService Implementation

        #region Private Functions
        private void SpatialPersistenceEvents(ISpatialPersistenceServiceModule provider, bool registerEvents)
        {
            if (registerEvents)
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
        #endregion Private Functions
    }
}