// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using XRTK.Interfaces.CameraSystem;

namespace XRTK.Interfaces.SpatialPersistence
{
    /// <summary>
    /// Provider agnostic Interface contract for Cloud SpatialPersistence system integration</see>.
    /// </summary>
    public interface IMixedRealitySpatialPersistenceSystem : IMixedRealityExtensionService
    {
        /// <summary>
        /// The current <see cref="IMixedRealitySpatialPersistenceDataProvider"/>
        /// </summary>
        IMixedRealitySpatialPersistenceDataProvider CurrentSpatialPersistenceProvider { get; }

        #region Public Methods

        /// <summary>
        /// Create a cloud SpatialPersistence at a specified location using the slected prefab GameObject
        /// </summary>
        /// <param name="objectToSpatialPersistencePrefab">Prefab to place in the scene</param>
        /// <param name="position">Raycast position to place the prefab and localise the Cloud SpatialPersistence</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localise the Cloud SpatialPersistence</param>
        /// <param name="timeToLive">Defined lifetime of the placed Cloud SpatialPersistence, informs the backend service to set a cache retention timeout</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement</remarks>
        void CreateAnchoredObject(GameObject objectToPlace, Vector3 position, Quaternion rotation, DateTimeOffset timeToLive);

        /// <summary>
        /// Instruct the cloud provider to locate an individual Cloud SpatialPersistence by its ID/UID
        /// </summary>
        /// <param name="id">String identifier for the cloud SpatialPersistence platform to locate</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return an SpatialPersistence, the <see cref="CloudAnchorLocated"/> event will respond with discovered SpatialPersistences</remarks>
        bool FindAnchorPoint(string id);

        /// <summary>
        /// Instruct the cloud provider to locate a collection of Cloud SpatialPersistences by their ID/UID
        /// </summary>
        /// <param name="ids">Array of string identifiers for the cloud SpatialPersistence platform to locate</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return SpatialPersistences, the <see cref="CloudAnchorLocated"/> event will respond with discovered SpatialPersistences</remarks>
        bool FindAnchorPoints(string[] ids);

        /// <summary>
        /// Places a GameObject or Prefab in the scene and wires up the CLoud SpatialPersistence with its localised position
        /// </summary>
        /// <param name="id">String identifier for the cloud SpatialPersistence platform to place</param>
        /// <param name="objectToSpatialPersistencePrefab">Prefab to place in the scene</param>
        /// <returns>Returns true of the object placement and SpatialPersistence hook was successful, fails if the SpatialPersistence ID is unknown</returns>
        bool PlaceSpatialPersistence(string id, GameObject objectToPlace);

        /// <summary>
        /// Moves a currently SpatialPersistenceed object to a new localised position.
        /// *Note, this in effect destroys the old SpatialPersistence and creates a new one.
        /// </summary>
        /// <param name="anchoredObject">Object in the scene to move</param>
        /// <param name="position">Raycast position to move the prefab to and relocalise the Cloud SpatialPersistence</param>
        /// <param name="rotation">Raycast rotation to move the prefab to and relocalise the Cloud SpatialPersistence</param>
        /// <param name="cloudSpatialPersistenceID">String identifier for the cloud SpatialPersistence platform to place</param>
        /// <returns></returns>
        bool MoveSpatialPersistence(GameObject anchoredObject, Vector3 position, Quaternion rotation, string cloudSpatialPersistenceID = "");

        /// <summary>
        /// Clear the current cache of located Cloud Anchors from the provider services
        /// </summary>
        /// <returns></returns>
        bool TryClearAnchors();

        #endregion Public Methods

        #region Public Events

        /// <summary>
        /// The SpatialPersistence provider reports that creation of the SpatialPersistence failed
        /// </summary>
        event Action CreateAnchoredObjectFailed;

        /// <summary>
        /// The SpatialPersistence provider reports that creation of the SpatialPersistence succeeded
        /// </summary>
        event Action<string, GameObject> CreateAnchoredObjectSucceeded;

        /// <summary>
        /// Status message whilst the SpatialPersistence service is localising the SpatialPersistence in place, continues until complete or a failure occurs
        /// </summary>
        event Action<string> SpatialPersistenceStatusMessage;

        /// <summary>
        /// General service failure from the SpatialPersistence provider
        /// </summary>
        event Action<string> SpatialPersistenceError;

        /// <summary>
        /// Notification that the provider has performed an operation on an object in the scene
        /// </summary>
        event Action<string, GameObject> CloudAnchorUpdated;

        /// <summary>
        /// Location request to SpatialPersistence service successful and a localised SpatialPersistence was found and cached.
        /// </summary>
        event Action<string> CloudAnchorLocated;

        #endregion Public Events

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
