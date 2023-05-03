// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Modules;
using RealityToolkit.SpatialPersistence.Definitions;
using RealityToolkit.SpatialPersistence.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence
{
    public class BaseSpatialPersistenceServiceModule : BaseServiceModule, ISpatialPersistenceServiceModule
    {
        #region Constructor
        public BaseSpatialPersistenceServiceModule(string name, uint priority, BaseProfile profile, ISpatialPersistenceService parentService)
            : base(name, priority, null, parentService)
        { }
        #endregion Constructor

        #region ISpatialPersistenceServiceModule Implementation
        /// <inheritdoc />
        public virtual bool IsRunning => false;

        /// <inheritdoc />
        public virtual SpatialPersistenceTrackingType TrackingType => SpatialPersistenceTrackingType.NotSupported;

        /// <inheritdoc />
        public virtual Task StartSpatialPersistenceModule()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void StopSpatialPersistenceModule()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual Task<Guid> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void TryFindAnchors(params Guid[] ids)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void TryFindAnchors(params SpatialPersistenceAnchorArgs[] args)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void TryFindAnchors(SpatialPersistenceSearchType searchType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual Task<bool> TryFindAnchorsAsync(params Guid[] ids)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual Task<bool> TryFindAnchorsAsync(params SpatialPersistenceAnchorArgs[] args)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool HasAnchor(GameObject anchoredObject)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryMoveAnchor(GameObject anchoredObject, Vector3 position, Quaternion rotation, Guid cloudAnchorID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void DeleteAnchors(params Guid[] ids)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void ResetAnchors(params Guid[] ids)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryClearAnchorCache()
        {
            throw new NotImplementedException();
        }
        #region Events

        #region Service Module Events

        /// <inheritdoc />
        public event Action SessionInitialized;

        /// <inheritdoc />
        public event Action SessionStarted;

        /// <inheritdoc />
        public event Action SessionEnded;

        /// <inheritdoc />
        public event Action CreateAnchorStarted;

        /// <inheritdoc />
        public event Action FindAnchorStarted;

        #region Handlers
        public void OnSessionInitialized() => SessionInitialized?.Invoke();
        public void OnSessionStarted() => SessionStarted?.Invoke();
        public void OnSessionEnded() => SessionEnded?.Invoke();
        public void OnCreateAnchorStarted() => CreateAnchorStarted?.Invoke();
        public void OnFindAnchorStarted() => FindAnchorStarted?.Invoke();
        #endregion Handlers

        #endregion Service Module Events

        #region Service Events
        /// <inheritdoc />
        public event Action CreateAnchorFailed;

        /// <inheritdoc />
        public event Action<Guid, GameObject> CreateAnchorSucceeded;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceStatusMessage;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceError;

        /// <inheritdoc />
        public event Action<Guid, GameObject> AnchorLocated;

        /// <inheritdoc />
        public event Action<Guid, string> AnchorLocatedError;

        /// <inheritdoc />
        public event Action<Guid, GameObject> AnchorUpdated;

        /// <inheritdoc />
        public event Action<Guid> AnchorDeleted;

        #region Handlers
        public void OnCreateAnchorFailed() => CreateAnchorFailed?.Invoke();
        public void OnCreateAnchorSucceeded(Guid guid, GameObject target) => CreateAnchorSucceeded?.Invoke(guid, target);
        public void OnSpatialPersistenceStatusMessage(string message) => SpatialPersistenceStatusMessage?.Invoke(message);
        public void OnSpatialPersistenceError(string message) => SpatialPersistenceError?.Invoke(message);
        public void OnAnchorLocated(Guid guid, GameObject target) => AnchorLocated?.Invoke(guid, target);
        public void OnAnchorLocatedError(Guid guid, string message) => AnchorLocatedError?.Invoke(guid, message);
        public void OnAnchorUpdated(Guid guid, GameObject target) => AnchorUpdated?.Invoke(guid, target);
        public void OnAnchorDeleted(Guid guid) => AnchorDeleted?.Invoke(guid);
        #endregion Handlers

        #endregion Service Events

        #endregion Events

        #endregion ISpatialPersistenceServiceModule Implementation
    }
}