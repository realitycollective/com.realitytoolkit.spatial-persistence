// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityCollective.ServiceFramework.Interfaces;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.SpatialPersistence.Definitions;
using RealityToolkit.SpatialPersistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var destroyingServiceModules = ServiceModules.ToArray();
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in destroyingServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                persistenceServiceModule.StopSpatialPersistenceModule();
                UnRegisterServiceModule(persistenceServiceModule);
            }
            base.Destroy();
        }
        #endregion MonoBehaviours

        #region IService
        public override bool RegisterServiceModules => false;
        #endregion IService

        #region ISpatialPersistenceService Implementation
        /// <inheritdoc />
        public async Task StartSpatialPersistenceService()
        {
            if (ServiceModules.Count > 0)
            {
                foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
                {
                    await persistenceServiceModule.StartSpatialPersistenceModule();
                }
            }
        }

        /// <inheritdoc />
        public void StopSpatialPersistenceService()
        {
            if (ServiceModules.Count > 0)
            {
                foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
                {
                    persistenceServiceModule.StopSpatialPersistenceModule();
                }
            }
        }

        /// <inheritdoc />
        public bool TryGetModulesByTrackingType(SpatialPersistenceTrackingType trackingType, out ISpatialPersistenceServiceModule[] modules)
        {
            var foundModules = new List<ISpatialPersistenceServiceModule>();
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                if (persistenceServiceModule.TrackingType == trackingType)
                {
                    foundModules.Add(persistenceServiceModule);
                }
            }
            modules = foundModules.ToArray();
            return foundModules.Count > 0;
        }

        /// <inheritdoc />
        public void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                if (persistenceServiceModule.TrackingType == SpatialPersistenceTrackingType.CloudAnchor)
                {
                    persistenceServiceModule.TryCreateAnchor(position, rotation, timeToLive);
                }
            }
        }

        /// <inheritdoc />
        public async Task<string> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                if (persistenceServiceModule.TrackingType == SpatialPersistenceTrackingType.CloudAnchor)
                {
                    return await persistenceServiceModule.TryCreateAnchorAsync(position, rotation, timeToLive);
                }
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public void TryFindAnchors(params SpatialPersistenceSearchArgs[] searchCriteria)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                if (persistenceServiceModule.TrackingType == SpatialPersistenceTrackingType.ImageTracking)
                {
                    persistenceServiceModule.TryFindAnchors(searchCriteria);
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> TryFindAnchorsAsync(params SpatialPersistenceSearchArgs[] searchCriteria)
        {
            Debug.Assert(searchCriteria != null, "Search criteria array is null");
            Debug.Assert(searchCriteria.Length > 0, "Search criteria required for SpatialPersistence search");

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                return await persistenceServiceModule.TryFindAnchorsAsync(searchCriteria);
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryMoveAnchor(GameObject anchoredObject, Vector3 worldPos, Quaternion worldRot, string cloudAnchorID)
        {
            Debug.Assert(anchoredObject != null, "Currently Anchored GameObject reference required");

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                if (persistenceServiceModule.TrackingType == SpatialPersistenceTrackingType.CloudAnchor)
                {
                    if (persistenceServiceModule.TryMoveAnchor(anchoredObject, worldPos, worldRot, cloudAnchorID))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <inheritdoc />
        public void TryDeleteAnchors(params string[] ids)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                if (persistenceServiceModule.TrackingType == SpatialPersistenceTrackingType.CloudAnchor)
                {
                    persistenceServiceModule.DeleteAnchors(ids);
                }
            }
        }

        /// <inheritdoc />
        public bool TryClearAnchorCache()
        {
            var anyClear = false;

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                if (persistenceServiceModule.TryClearAnchorCache())
                {
                    anyClear = true;
                }
            }

            return anyClear;
        }
        
        public void CancelAnchorOperation()
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules.Cast<ISpatialPersistenceServiceModule>())
            {
                persistenceServiceModule.CancelAnchorOperation();
            }
        }

        /// <inheritdoc />
        public event Action CreateAnchorFailed;
        private void OnCreateAnchorFailed() => CreateAnchorFailed?.Invoke();

        /// <inheritdoc />
        public event Action<string, GameObject> CreateAnchorSucceeded;
        private void OnCreateAnchorSucceeded(string id, GameObject anchoredObject) => CreateAnchorSucceeded?.Invoke(id, anchoredObject);

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceStatusMessage;
        private void OnSpatialPersistenceStatusMessage(string message) => SpatialPersistenceStatusMessage?.Invoke(message);

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceError;
        private void OnSpatialPersistenceError(string exception) => SpatialPersistenceError?.Invoke(exception);

        /// <inheritdoc />
        public event Action<string, GameObject> AnchorLocated;
        private void OnAnchorLocated(string id, GameObject anchoredGameObject) => AnchorLocated?.Invoke(id, anchoredGameObject);

        /// <inheritdoc />
        public event Action<string, string> AnchorLocatedError;
        private void OnAnchorLocatedError(string id, string exception) => AnchorLocatedError?.Invoke(id, exception);

        /// <inheritdoc />
        public event Action<string, GameObject> AnchorUpdated;
        private void OnAnchorUpdated(string id, GameObject gameObject) => AnchorUpdated?.Invoke(id, gameObject);

        /// <inheritdoc />
        public event Action<string> AnchorDeleted;
        private void OnAnchorDeleted(string id) => AnchorDeleted?.Invoke(id);

        #endregion ISpatialPersistenceService Implementation

        #region BaseService Implementation
        /// <inheritdoc />
        public override void RegisterServiceModule(IServiceModule serviceModule)
        {
            base.RegisterServiceModule(serviceModule);

            SpatialPersistenceEvents(serviceModule as ISpatialPersistenceServiceModule, true);
            if (autoStartBehavior == AutoStartBehavior.AutoStart)
            {
                (serviceModule as ISpatialPersistenceServiceModule).StartSpatialPersistenceModule();
            }
        }

        /// <inheritdoc />
        public override void UnRegisterServiceModule(IServiceModule serviceModule)
        {
            SpatialPersistenceEvents(serviceModule as ISpatialPersistenceServiceModule, false);
            (serviceModule as ISpatialPersistenceServiceModule).StopSpatialPersistenceModule();
            base.UnRegisterServiceModule(serviceModule);
        }
        #endregion BaseService Implementation

        #region Private Functions
        private void SpatialPersistenceEvents(ISpatialPersistenceServiceModule module, bool registerEvents)
        {
            if (registerEvents)
            {
                module.CreateAnchorSucceeded += OnCreateAnchorSucceeded;
                module.CreateAnchorFailed += OnCreateAnchorFailed;
                module.SpatialPersistenceStatusMessage += OnSpatialPersistenceStatusMessage;
                module.AnchorLocated += OnAnchorLocated;
                module.AnchorLocatedError += OnAnchorLocatedError;
                module.AnchorUpdated += OnAnchorUpdated;
                module.AnchorDeleted += OnAnchorDeleted;
                module.SpatialPersistenceError += OnSpatialPersistenceError;
            }
            else
            {
                module.CreateAnchorSucceeded -= OnCreateAnchorSucceeded;
                module.CreateAnchorFailed -= OnCreateAnchorFailed;
                module.SpatialPersistenceStatusMessage -= OnSpatialPersistenceStatusMessage;
                module.AnchorLocated -= OnAnchorLocated;
                module.AnchorLocatedError -= OnAnchorLocatedError;
                module.AnchorUpdated -= OnAnchorUpdated;
                module.AnchorDeleted -= OnAnchorDeleted;
                module.SpatialPersistenceError -= OnSpatialPersistenceError;
            }
        }
        #endregion Private Functions
    }
}