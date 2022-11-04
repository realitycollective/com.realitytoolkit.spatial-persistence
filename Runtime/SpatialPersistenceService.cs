// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Definitions.Utilities;
using RealityCollective.ServiceFramework.Interfaces;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.SpatialPersistence.Definitions;
using RealityToolkit.SpatialPersistence.Interfaces;
using System;
using System.Collections.Generic;
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
                persistenceServiceModule.StopSpatialPersistenceModule();
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
                foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
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
                foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
                {
                    persistenceServiceModule.StopSpatialPersistenceModule();
                }
            }
        }

        /// <inheritdoc />
        public bool TryGetModulesByTrackingType(SpatialPersistenceTrackingType trackingType, out ISpatialPersistenceServiceModule[] modules)
        {
            var foundModules = new List<ISpatialPersistenceServiceModule>();
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
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
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.TryCreateAnchor(position, rotation, timeToLive);
            }
        }

        /// <inheritdoc />
        public async Task<Guid> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                return await persistenceServiceModule.TryCreateAnchorAsync(position, rotation, timeToLive);
            }

            return Guid.Empty;
        }

        /// <inheritdoc />
        public void TryFindAnchors(params Guid[] ids)
        {
            Debug.Assert(ids != null, "ID array is null");
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.TryFindAnchors(ids);
            }
        }

        /// <inheritdoc />
        public void TryFindAnchors(params SpatialPersistenceAnchorArgs[] args)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.TryFindAnchors(args);
            }
        }

        /// <inheritdoc />
        public void TryFindAnchors(SpatialPersistenceSearchType searchType)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.TryFindAnchors(searchType);
            }
        }

        /// <inheritdoc />
        public async Task<bool> TryFindAnchorsAsync(params Guid[] ids)
        {
            Debug.Assert(ids != null, "ID array is null");
            Debug.Assert(ids.Length > 0, "IDs required for SpatialPersistence search");

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                return await persistenceServiceModule.TryFindAnchorsAsync(ids);
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<bool> TryFindAnchorsAsync(params SpatialPersistenceAnchorArgs[] args)
        {
            Debug.Assert(args != null, "ID array is null");
            Debug.Assert(args.Length > 0, "IDs required for SpatialPersistence search");

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                return await persistenceServiceModule.TryFindAnchorsAsync(args);
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryMoveAnchor(GameObject anchoredObject, Vector3 worldPos, Quaternion worldRot, Guid cloudAnchorID)
        {
            Debug.Assert(anchoredObject != null, "Currently Anchored GameObject reference required");

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                if (persistenceServiceModule.TryMoveAnchor(anchoredObject, worldPos, worldRot, cloudAnchorID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public void TryDeleteAnchors(params Guid[] ids)
        {
            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                persistenceServiceModule.DeleteAnchors(ids);
            }
        }

        /// <inheritdoc />
        public bool TryClearAnchorCache()
        {
            var anyClear = false;

            foreach (ISpatialPersistenceServiceModule persistenceServiceModule in ServiceModules)
            {
                if (persistenceServiceModule.TryClearAnchorCache())
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
                module.AnchorUpdated += OnAnchorUpdated;
                module.AnchorLocated += OnAnchorLocated;
                module.AnchorLocatedError += OnAnchorLocatedError;
                module.SpatialPersistenceError += OnSpatialPersistenceError;
            }
            else
            {
                module.CreateAnchorSucceeded -= OnCreateAnchorSucceeded;
                module.CreateAnchorFailed -= OnCreateAnchorFailed;
                module.SpatialPersistenceStatusMessage -= OnSpatialPersistenceStatusMessage;
                module.AnchorUpdated -= OnAnchorUpdated;
                module.AnchorLocated -= OnAnchorLocated;
                module.AnchorLocatedError -= OnAnchorLocatedError;
                module.SpatialPersistenceError -= OnSpatialPersistenceError;
            }
        }
        #endregion Private Functions
    }
}