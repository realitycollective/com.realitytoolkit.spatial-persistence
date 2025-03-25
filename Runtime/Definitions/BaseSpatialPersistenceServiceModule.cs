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
        public virtual Task<string> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void TryFindAnchors(params SpatialPersistenceSearchArgs[] searchCriteria)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual Task<bool> TryFindAnchorsAsync(params SpatialPersistenceSearchArgs[] args)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool HasAnchor(GameObject anchoredObject)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryMoveAnchor(GameObject anchoredObject, Vector3 position, Quaternion rotation, string cloudAnchorID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void DeleteAnchors(params string[] ids)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void ResetAnchors(params string[] ids)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryClearAnchorCache()
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc />
        public virtual void CancelAnchorOperation()
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
        public event Action<string, GameObject> CreateAnchorSucceeded;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceStatusMessage;

        /// <inheritdoc />
        public event Action<string> SpatialPersistenceError;

        /// <inheritdoc />
        public event Action<string, GameObject> AnchorLocated;

        /// <inheritdoc />
        public event Action<string, string> AnchorLocatedError;

        /// <inheritdoc />
        public event Action<string, GameObject> AnchorUpdated;

        /// <inheritdoc />
        public event Action<string> AnchorDeleted;

        #region Handlers
        public void OnCreateAnchorFailed() => CreateAnchorFailed?.Invoke();
        public void OnCreateAnchorSucceeded(string anchorID, GameObject target) => CreateAnchorSucceeded?.Invoke(anchorID, target);
        public void OnSpatialPersistenceStatusMessage(string message) => SpatialPersistenceStatusMessage?.Invoke(message);
        public void OnSpatialPersistenceError(string message) => SpatialPersistenceError?.Invoke(message);
        public void OnAnchorLocated(string anchorID, GameObject target) => AnchorLocated?.Invoke(anchorID, target);
        public void OnAnchorLocatedError(string anchorID, string message) => AnchorLocatedError?.Invoke(anchorID, message);
        public void OnAnchorUpdated(string anchorID, GameObject target) => AnchorUpdated?.Invoke(anchorID, target);
        public void OnAnchorDeleted(string anchorID) => AnchorDeleted?.Invoke(anchorID);

        #endregion Handlers

        #endregion Service Events

        #endregion Events

        #endregion ISpatialPersistenceServiceModule Implementation
    }
}