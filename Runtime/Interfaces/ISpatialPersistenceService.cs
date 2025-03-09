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
    /// Vendor agnostic Interface contract for SpatialPersistence system integration.
    /// </summary>
    public interface ISpatialPersistenceService : IService
    {
        #region Methods
        /// <summary>
        /// Command the SpatialPersistence service to connect and enter a running state.
        /// </summary>
        Task StartSpatialPersistenceService();

        /// <summary>
        /// Command the SpatialPersistence service to stop and disconnect from its backend vendor solution.
        /// </summary>
        void StopSpatialPersistenceService();

        /// <summary>
        /// Get all the currently registered modules utilizing the same tracking type.
        /// </summary>
        /// <param name="trackingType">How does the Spatial Persistence module locate its anchors?</param>
        /// <param name="modules">Array of <see cref="ISpatialPersistenceServiceModule"/> modules that implement the selected <see cref="SpatialPersistenceTrackingType"/>.</param>
        /// <returns>Returns true of there are modules registered of the selected type.</returns>
        bool TryGetModulesByTrackingType(SpatialPersistenceTrackingType trackingType, out ISpatialPersistenceServiceModule[] modules);

        /// <summary>
        /// Create an Anchor at a specified location.
        /// </summary>
        /// <param name="position">Raycast position to place the prefab and localize the Anchor.</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localize the Anchor.</param>
        /// <param name="timeToLive">Defined lifetime of the placed Anchor, informs the backend service to set a cache retention timeout, if a negative value is used, this is interpreted as being for an indefinite time period.</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement.</remarks>
        void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Create an Anchor at a specified location.
        /// </summary>
        /// <param name="position">Raycast position to place the prefab and localize the Anchor.</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localize the Anchor.</param>
        /// <param name="timeToLive">Defined lifetime of the placed Anchor, informs the backend service to set a cache retention timeout, if a negative value is used, this is interpreted as being for an indefinite time period.</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement.</remarks>
        Task<string> TryCreateAnchorAsync(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors by their Image for a specified ID/UID.
        /// </summary>
        /// <param name="ids">Array of <see cref="SpatialPersistenceSearchArgs"/> identifiers/Texture pairs for the SpatialPersistence platform to locate.</param>
        /// <returns>Returns true of the location request to the service was successful.</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        void TryFindAnchors(params SpatialPersistenceSearchArgs[] args);

        /// <summary>
        /// Instruct the vendor solution to locate a collection of Anchors by their Image for a specified ID/UID ID/UID async.
        /// </summary>
        /// <param name="ids">Array of  <see cref="SpatialPersistenceSearchArgs"/> identifiers/Texture pairs for the SpatialPersistence platform to locate.</param>
        /// <returns>Returns true of the location request to the service was successful.</returns>
        /// <remarks>Does not return anchored objects, the <see cref="AnchorLocated"/> event will respond with discovered Anchors.</remarks>
        Task<bool> TryFindAnchorsAsync(params SpatialPersistenceSearchArgs[] args);

        /// <summary>
        /// Moves a currently anchored object to a new localized position.
        /// *Note, this in effect destroys the old Anchor and creates a new one.
        /// </summary>
        /// <param name="anchoredObject">Object in the scene to move.</param>
        /// <param name="position">Raycast position to move the prefab to and re-localize the Anchor.</param>
        /// <param name="rotation">Raycast rotation to move the prefab to and re-localize the Anchor.</param>
        /// <param name="vendorAnchorID"><see cref="string"/> identifier for the Anchor platform to place.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        bool TryMoveAnchor(GameObject anchoredObject, Vector3 position, Quaternion rotation, string vendorAnchorID);

        /// <summary>
        /// Instruct the vendor solution to delete a collection of Anchors by their ID/UID.
        /// </summary>
        /// <param name="ids">list of anchors to remove.</param>
        void TryDeleteAnchors(params string[] ids);

        /// <summary>
        /// Instruct the Anchor system to clear its cache of downloaded Anchors.  Does not delete the Anchors from the vendor solution.
        /// </summary>
        /// <returns>Returns true if the operation was successful.</returns>
        bool TryClearAnchorCache();

        /// <summary>
        /// Instruct the vendor solution to cancel the current Anchor operation.
        /// This is usually a long running operation, such as creating an Anchor or locating an Anchor.
        /// </summary>
        void CancelAnchorOperation();

        #endregion Methods

        #region Events

        /// <summary>
        /// The Spatial Persistence vendor reports that creation of the Anchor failed
        /// </summary>
        event Action CreateAnchorFailed;

        /// <summary>
        /// The Spatial Persistence vendor reports that creation of the Anchor succeeded
        /// </summary>
        event Action<string, GameObject> CreateAnchorSucceeded;

        /// <summary>
        /// Status message whilst the Spatial Persistence service is localizing the Anchor in place, continues until complete or a failure occurs
        /// </summary>
        event Action<string> SpatialPersistenceStatusMessage;

        /// <summary>
        /// General service failure from the Anchor vendor
        /// </summary>
        event Action<string> SpatialPersistenceError;

        /// <summary>
        /// Location request to Spatial Persistence service successful and a localized Anchor was found and cached.
        /// </summary>
        event Action<string, GameObject> AnchorLocated;

        /// <summary>
        /// An error occurred retrieving the selected Anchor and the data was invalid.
        /// </summary>
        event Action<string, string> AnchorLocatedError;

        /// <summary>
        /// Notification that the vendor solution has performed an operation on an object in the scene
        /// </summary>
        event Action<string, GameObject> AnchorUpdated;

        /// <summary>
        /// Notification that the vendor solution has stopped tracking an object in the scene
        /// </summary>
        event Action<string> AnchorDeleted;
        #endregion Events
    }
}