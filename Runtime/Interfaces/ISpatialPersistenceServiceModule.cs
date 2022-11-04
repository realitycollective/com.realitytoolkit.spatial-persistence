// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Interfaces;
using RealityToolkit.SpatialPersistence.Definitions;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RealityToolkit.SpatialPersistence.Interfaces
{
    /// <summary>
    /// Interface contract for specific identity vendor implementations for use in the <see cref="ISpatialPersistenceService"/>.
    /// </summary>
    public interface ISpatialPersistenceServiceModule : IServiceModule
    {
        #region Public Properties
        /// <summary>
        /// Is the current Spatial Persistence module running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// The anchor tracking method of spatial persistence service.
        /// </summary>
        /// <remarks>
        /// Be careful of registering more that one of each <see cref="SpatialPersistenceTrackingType"/>, as this could cause conflicts.
        /// </remarks>
        SpatialPersistenceTrackingType TrackingType { get; }
        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Command the SpatialPersistence service module to connect and enter a running state.
        /// </summary>
        Task StartSpatialPersistenceModule();

        /// <summary>
        /// Command the SpatialPersistence service module to stop and disconnect from its backend vendor solution.
        /// </summary>
        void StopSpatialPersistenceModule();

        /// <summary>
        /// Create an Anchor at a specified location.
        /// </summary>
        /// <param name="position">Raycast position to place the prefab and localize the Anchor.</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localize the Anchor.</param>
        /// <param name="timeToLive">Defined lifetime of the placed Anchor, informs the backend service to set a cache retention timeout, if a negative value is used, this is interpreted as being for an indefinite time period.</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement.</remarks>
        void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Create a anchor Anchor at a specified location async
        /// </summary>
        /// <param name="position">Raycast position to place the prefab and localize the Anchor.</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localize the Anchor.</param>
        /// <param name="timeToLive">Defined lifetime of the placed Anchor, informs the backend service to set a cache retention timeout, if a negative value is used, this is interpreted as being for an indefinite time period.</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement.</remarks>
        Task<Guid> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors by their ID/UID.
        /// </summary>
        /// <param name="ids">Array of <see cref="Guid"/> identifiers for the SpatialPersistence platform to locate.</param>
        /// <returns>Returns true of the location request to the service was successful.</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        void TryFindAnchors(params Guid[] ids);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors by their Image for a specified ID/UID.
        /// </summary>
        /// <param name="ids">Array of <see cref="Guid"/> identifiers for the SpatialPersistence platform to locate.</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        void TryFindAnchors(params SpatialPersistenceAnchorArgs[] args);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors using a specific type of search, e.g. Nearby.
        /// </summary>
        /// <param name="searchType">The type of search to perform, specified by the <see cref="SpatialPersistenceSearchType"/> type.</param>
        /// <returns>Returns true of the location request to the service was successful.</returns>
        /// <remarks>Does not return Anchors, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        void TryFindAnchors(SpatialPersistenceSearchType searchType);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors by their ID/UID async.
        /// </summary>
        /// <param name="ids">Array of <see cref="Guid"/> identifiers for the SpatialPersistence platform to locate.</param>
        /// <returns>Returns true of the location request to the service was successful.</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        Task<bool> TryFindAnchorsAsync(params Guid[] ids);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors by their Image for a specified ID/UID ID/UID async.
        /// </summary>
        /// <param name="ids">Array of <see cref="Guid"/> identifiers for the SpatialPersistence platform to locate.</param>
        /// <returns>Returns true of the location request to the service was successful.</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        Task<bool> TryFindAnchorsAsync(params SpatialPersistenceAnchorArgs[] args);

        /// <summary>
        /// Does the selected GameObject currently have an Anchor attached.
        /// </summary>
        /// <param name="anchoredObject">The <see cref="GameObject"/> to check if it has an anchored component.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        bool HasAnchor(GameObject anchoredObject);

        /// <summary>
        /// Move Native SpatialPersistence anchor to a new position.
        /// </summary>
        /// <remarks>
        /// Moving an object with an existing Anchor without referencing its cached ID destroys the vendor solution position for the object.
        /// </remarks>
        /// <param name="anchoredObject">Existing GameObject reference of an object to move.</param>
        /// <param name="position">New world position to move to.</param>
        /// <param name="rotation">New rotation to apply.</param>
        /// <param name="vendorAnchorID">Preexisting vendor SpatialPersistenceID.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        bool TryMoveAnchor(GameObject anchoredObject, Vector3 position, Quaternion rotation, Guid vendorAnchorID);

        /// <summary>
        /// Instruct the vendor solution to delete a collection of Anchors by their ID/UID.
        /// </summary>
        /// <param name="ids">list of anchors to remove.</param>
        void DeleteAnchors(params Guid[] ids);

        /// <summary>
        /// Instruct the Anchor system to clear its cache of downloaded Anchors.  Does not delete the Anchors from the vendor solution.
        /// </summary>
        /// <returns>Returns true if the operation was successful.</returns>
        bool TryClearAnchorCache();

        #endregion Public Methods

        #region Internal Anchor Module Events

        /// <summary>
        /// Event fired when the Vendor Solution has finished initializing
        /// </summary>
        event Action SessionInitialized;

        /// <summary>
        /// Event fired when the Vendor Solution has begun searching for Anchors
        /// </summary>
        event Action SessionStarted;

        /// <summary>
        /// Event fired when the Vendor Solution has stopped searching for Anchors
        /// </summary>
        event Action SessionEnded;

        /// <summary>
        /// Event fired when the Vendor Solution has started creating an Anchor.  Finishes when the Anchor is located.
        /// </summary>
        event Action CreateAnchorStarted;

        /// <summary>
        /// Event fired when the Vendor Solution has started searching for Anchors.  <see cref="AnchorLocated"/> event fired for each Anchor found.
        /// </summary>
        event Action FindAnchorStarted;

        /// <summary>
        /// Event fired when the Vendor Solution has deleted an Anchor.
        /// </summary>
        event Action<Guid> AnchorDeleted;

        #endregion Internal Anchor Module Events

        #region Spatial Persistence Service Events

        /// <summary>
        /// The Spatial Persistence vendor reports that creation of the Anchor failed
        /// </summary>
        event Action CreateAnchorFailed;

        /// <summary>
        /// The Spatial Persistence vendor reports that creation of the Anchor succeeded
        /// </summary>
        event Action<Guid, GameObject> CreateAnchorSucceeded;

        /// <summary>
        /// Status message whilst the Spatial Persistence service is localizing the Anchor in place, continues until complete or a failure occurs
        /// </summary>
        event Action<string> SpatialPersistenceStatusMessage;

        /// <summary>
        /// General service failure from the Anchor vendor
        /// </summary>
        event Action<string> SpatialPersistenceError;

        /// <summary>
        /// Notification that the vendor solution has performed an operation on an object in the scene
        /// </summary>
        event Action<Guid, GameObject> AnchorUpdated;

        /// <summary>
        /// Location request to Spatial Persistence service successful and a localized Anchor was found and cached.
        /// </summary>
        event Action<Guid, GameObject> AnchorLocated;

        /// <summary>
        /// An error occurred retrieving the selected Anchor and the data was invalid.
        /// </summary>
        event Action<Guid, string> AnchorLocatedError;

        #endregion Spatial Persistence Service Events
    }
}