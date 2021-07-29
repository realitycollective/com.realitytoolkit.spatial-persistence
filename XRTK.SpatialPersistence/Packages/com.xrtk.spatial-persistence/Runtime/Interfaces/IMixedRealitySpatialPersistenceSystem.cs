// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace XRTK.Interfaces.SpatialPersistence
{
    /// <summary>
    /// Provider agnostic Interface contract for Cloud SpatialPersistence system integration</see>.
    /// </summary>
    public interface IMixedRealitySpatialPersistenceSystem : IMixedRealitySystem
    {
        #region Methods

        /// <summary>
        /// Create a cloud Anchor at a specified location using the slected prefab GameObject
        /// </summary>
        /// <param name="position">Raycast position to place the prefab and localise the Cloud Anchor</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localise the Cloud Anchor</param>
        /// <param name="timeToLive">Defined lifetime of the placed Cloud Anchor, informs the backend service to set a cache retention timeout</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement</remarks>
        void TryCreateAnchor(Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Instruct the cloud provider to locate a collection of Cloud Anchors by their ID/UID
        /// </summary>
        /// <param name="ids">Array of <see cref="Guid"/> identifiers for the cloud Anchor platform to locate</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return Anchors, the <see cref="AnchorLocated"/> event will respond with discovered Anchors</remarks>
        bool TryFindAnchorPoints(params Guid[] ids);

        /// <summary>
        /// Moves a currently SpatialPersistenceed object to a new localised position.
        /// *Note, this in effect destroys the old Anchor and creates a new one.
        /// </summary>
        /// <param name="anchoredObject">Object in the scene to move</param>
        /// <param name="position">Raycast position to move the prefab to and relocalise the Cloud Anchor</param>
        /// <param name="rotation">Raycast rotation to move the prefab to and relocalise the Cloud Anchor</param>
        /// <param name="cloudAnchorID"><see cref="Guid"/> identifier for the cloud Anchor platform to place</param>
        /// <returns></returns>
        bool TryMoveSpatialPersistence(GameObject anchoredObject, Vector3 position, Quaternion rotation, Guid cloudAnchorID = new Guid());

        /// <summary>
        /// Clear the current cache of located Cloud Anchors from the provider services
        /// </summary>
        /// <returns></returns>
        bool TryClearAnchorCache();

        #endregion Methods

        #region Events

        /// <summary>
        /// The SpatialPersistence provider reports that creation of the Anchor failed
        /// </summary>
        event Action CreateAnchorFailed;

        /// <summary>
        /// The SpatialPersistence provider reports that creation of the Anchor succeeded
        /// </summary>
        event Action<Guid, GameObject> CreateAnchorSucceeded;

        /// <summary>
        /// Status message whilst the SpatialPersistence service is localising the Anchor in place, continues until complete or a failure occurs
        /// </summary>
        event Action<string> SpatialPersistenceStatusMessage;

        /// <summary>
        /// General service failure from the SpatialPersistence provider
        /// </summary>
        event Action<string> SpatialPersistenceError;

        /// <summary>
        /// Notification that the provider has performed an operation on an object in the scene
        /// </summary>
        event Action<Guid, GameObject> AnchorUpdated;

        /// <summary>
        /// Location request to SpatialPersistence service successful and a localised Anchor was found and cached.
        /// </summary>
        event Action<Guid, GameObject> AnchorLocated;

        #endregion Events

        /// <summary>
        /// Registers the <see cref="IMixedRealitySpatialPersistenceDataProvider"/> with the <see cref="IMixedRealitySpatialPersistenceSystem"/>.
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        bool RegisterSpatialPersistenceDataProvider(IMixedRealitySpatialPersistenceDataProvider dataProvider);

        /// <summary>
        /// UnRegisters the <see cref="IMixedRealitySpatialPersistenceDataProvider"/> with the <see cref="IMixedRealitySpatialPersistenceSystem"/>.
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        bool UnRegisterSpatialPersistenceDataProvider(IMixedRealitySpatialPersistenceDataProvider dataProvider);
    }
}
