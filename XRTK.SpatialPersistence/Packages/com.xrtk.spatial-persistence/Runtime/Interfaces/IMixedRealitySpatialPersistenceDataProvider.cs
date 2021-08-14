// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using XRTK.Definitions.SpatialPersistence;

namespace XRTK.Interfaces.SpatialPersistence
{
    /// <summary>
    /// Interface contract for specific identity provider implementations for use in the <see cref="IMixedRealitySpatialPersistenceSystem"/>.
    /// </summary>
    public interface IMixedRealitySpatialPersistenceDataProvider : IMixedRealityDataProvider
    {
        /// <summary>
        /// Is the current Spatial Persistence provider running
        /// </summary>
        bool IsRunning { get; }

        #region Public Methods

        /// <summary>
        /// Command the SpatialPersistence service to connect and enter a running state
        /// </summary>
        void StartSpatialPersistenceProvider();

        /// <summary>
        /// Command the SpatialPersistence service to stop and disconnect from its cloud backend
        /// </summary>
        void StopSpatialPersistenceProvider();

        /// <summary>
        /// Create a cloud Anchor at a specified location
        /// </summary>
        /// <param name="position">Raycast position to place the prefab and localize the Cloud Anchor</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localize the Cloud Anchor</param>
        /// <param name="timeToLive">Defined lifetime of the placed Cloud Anchor, informs the backend service to set a cache retention timeout</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement</remarks>
        void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Instruct the cloud provider to locate a collection of Cloud Anchors by their ID/UID
        /// </summary>
        /// <param name="ids">Array of <see cref="Guid"/> identifiers for the cloud SpatialPersistence platform to locate</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors</remarks>
        bool TryFindAnchorPoints(params Guid[] ids);

        /// <summary>
        /// Instruct the cloud provider to locate a collection of Cloud Anchors using a specific type of search, e.g. Nearby
        /// </summary>
        /// <param name="searchType">The type of search to perform, specified by the <see cref="SpatialPersistenceSearchType"/> type</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return Anchors, the <see cref="AnchorLocated"/> event will respond with discovered Anchors</remarks>
        bool TryFindAnchorPoints(SpatialPersistenceSearchType searchType);

        /// <summary>
        /// Does the selected GameObject currently have an Anchor attached
        /// </summary>
        /// <param name="anchoredObject"></param>
        /// <returns></returns>
        bool HasCloudAnchor(GameObject anchoredObject);

        /// <summary>
        /// Move Native SpatialPersistence to a new position
        /// </summary>
        /// <remarks>
        /// Moving an object with an existing cloud Anchor without referencing it's cached ID destroys the cloud position for the object
        /// </remarks>
        /// <param name="anchoredObject">Existing GameObject reference of an object to move</param>
        /// <param name="position">New world position to move to</param>
        /// <param name="rotation">New rotation to apply</param>
        /// <param name="cloudAnchorID">Preexisting cloud SpatialPersistenceID</param>
        /// <returns></returns>
        bool TryMoveSpatialPersistence(GameObject anchoredObject, Vector3 position, Quaternion rotation, Guid cloudAnchorID = new Guid());

        /// <summary>
        /// Instruct the cloud provider to delete a collection of Cloud Anchors by their ID/UID
        /// </summary>
        /// <param name="ids"></param>
        void DeleteAnchors(params Guid[] ids);

        /// <summary>
        /// Instruct the Anchor system to clear its cache of downloaded Anchors.  Does not delete the Anchors from the cloud service
        /// </summary>
        /// <returns></returns>
        bool TryClearAnchorCache();

        #endregion Public Methods

        #region Internal Cloud Anchor Provider Events

        /// <summary>
        /// Event fired when the Cloud Provider has finished initializing
        /// </summary>
        event Action SessionInitialized;

        /// <summary>
        /// Event fired when the Cloud Provider has begun searching for Anchors
        /// </summary>
        event Action SessionStarted;

        /// <summary>
        /// Event fired when the Cloud Provider has stopped searching for Anchors
        /// </summary>
        event Action SessionEnded;

        /// <summary>
        /// Event fired when the Cloud Provider has started creating an Anchor.  Finishes when the Anchor is located.
        /// </summary>
        event Action CreateAnchorStarted;

        /// <summary>
        /// Event fired when the Cloud Provider has started searching for Anchors.  <see cref="AnchorLocated"/> event fired for each Anchor found.
        /// </summary>
        event Action FindAnchorStarted;

        /// <summary>
        /// Event fired when the Cloud Provider has deleted an Anchor.
        /// </summary>
        event Action<Guid> AnchorDeleted;

        #endregion Internal Cloud Anchor Provider Events

        #region Spatial Persistence Service Events

        /// <summary>
        /// The Spatial Persistence provider reports that creation of the Cloud Anchor failed
        /// </summary>
        event Action CreateAnchorFailed;

        /// <summary>
        /// The Spatial Persistence provider reports that creation of the Cloud Anchor succeeded
        /// </summary>
        event Action<Guid, GameObject> CreateAnchorSucceeded;

        /// <summary>
        /// Status message whilst the Spatial Persistence service is localizing the Cloud Anchor in place, continues until complete or a failure occurs
        /// </summary>
        event Action<string> SpatialPersistenceStatusMessage;

        /// <summary>
        /// General service failure from the Anchor provider
        /// </summary>
        event Action<string> SpatialPersistenceError;

        /// <summary>
        /// Notification that the provider has performed an operation on an object in the scene
        /// </summary>
        event Action<Guid, GameObject> AnchorUpdated;

        /// <summary>
        /// Location request to Spatial Persistence service successful and a localized Cloud Anchor was found and cached.
        /// </summary>
        event Action<Guid, GameObject> AnchorLocated;

        #endregion Spatial Persistence Service Events
    }
}
