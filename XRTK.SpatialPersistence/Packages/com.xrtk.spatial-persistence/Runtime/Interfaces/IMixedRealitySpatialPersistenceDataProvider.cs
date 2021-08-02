// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using XRTK.Definitions.SpatialPersistence;
using XRTK.Definitions.Utilities;

namespace XRTK.Interfaces.SpatialPersistence
{
    /// <summary>
    /// Interface contract for specific identity provider implementations for use in the <see cref="IMixedRealitySpatialPersistenceSystem"/>.
    /// </summary>
    public interface IMixedRealitySpatialPersistenceDataProvider : IMixedRealityDataProvider
    {
        /// <summary>
        /// The Instance Type for the Spatial Persistence Data Provider
        /// </summary>
        SystemType SpatialPersistenceType { get; }

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
        /// Create a cloud SpatialPersistence at a specified location using the slected prefab GameObject
        /// </summary>
        /// <param name="objectToPlace">Prefab to place in the scene</param>
        /// <param name="position">Raycast position to place the prefab and localise the Cloud SpatialPersistence</param>
        /// <param name="rotation">Raycast rotation to place the prefab and localise the Cloud SpatialPersistence</param>
        /// <param name="timeToLive">Defined lifetime of the placed Cloud SpatialPersistence, informs the backend service to set a cache retention timeout</param>
        /// <remarks>The Position and Rotation are usually the result of a Raycast hit in to the AR scene for placement</remarks>
        void CreateAnchoredObject(GameObject objectToPlace, Vector3 position, Quaternion rotation, System.DateTimeOffset timeToLive);

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
        /// Instruct the cloud provider to locate a collection of Cloud SpatialPersistences using a specific type of search, e.g. Nearby
        /// </summary>
        /// <param name="searchType">The type of search to perform, specified by the <see cref="SpatialPersistenceSearchType"/> type</param>
        /// <returns>Returns true of the location request to the service was successful</returns>
        /// <remarks>Does not return SpatialPersistences, the <see cref="CloudAnchorLocated"/> event will respond with discovered SpatialPersistences</remarks>
        bool FindAnchorPoints(SpatialPersistenceSearchType searchType);

        /// <summary>
        /// Places a GameObject or Prefab in the scene and wires up the CLoud Anchor with its localised position
        /// </summary>
        /// <param name="id">String identifier for the cloud SpatialPersistence platform to place</param>
        /// <param name="objectToSpatialPersistencePrefab">Prefab to place in the scene</param>
        /// <returns>Returns true of the object placement and SpatialPersistence hook was successful, fails if the SpatialPersistence ID is unknown</returns>
        bool PlaceAnchoredObject(string id, GameObject objectToPlace);

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
        /// Moving an object with an existing cloud SpatialPersistence without referencing it's cached ID destroys the cloud position for the object
        /// </remarks>
        /// <param name="anchoredObject">Existing GameObject reference of an object to move</param>
        /// <param name="worldPos">New world position to move to</param>
        /// <param name="worldRot">New rotation to apply</param>
        /// <param name="cloudSpatialPersistenceID">Preexisting cloud SpatialPersistenceID</param>
        /// <returns></returns>
        bool MoveAnchoredObject(GameObject anchoredObject, Vector3 position, Quaternion rotation, string cloudSpatialPersistenceID = "");

        /// <summary>
        /// Instruct the cloud provider to delete an individual Cloud SpatialPersistence by its ID/UID
        /// </summary>
        /// <param name="id"></param>
        void DeleteAnchor(string id);

        /// <summary>
        /// Instruct the cloud provider to delete a collection of Cloud SpatialPersistences by their ID/UID
        /// </summary>
        /// <param name="ids"></param>
        void DeleteAnchors(string[] ids);

        /// <summary>
        /// Instruct the SpatialPersistence system to clear its cache of downloaded SpatialPersistences.  Does not delete the SpatialPersistences from the cloud service
        /// </summary>
        /// <returns></returns>
        bool TryClearAnchors();

        #endregion Public Methods

        #region Internal Cloud Anchor Provider Events

        /// <summary>
        /// Event fired when the Cloud Provider has finished initialising
        /// </summary>
        event Action SessionInitialised;

        /// <summary>
        /// Event fired when the Cloud Provider has begun searching for SpatialPersistences
        /// </summary>
        event Action SessionStarted;

        /// <summary>
        /// Event fired when the Cloud Provider has stopped searching for SpatialPersistences
        /// </summary>
        event Action SessionEnded;

        /// <summary>
        /// Event fired when the Cloud Provider has started creating an SpatialPersistence.  Finishes when the SpatialPersistence is located.
        /// </summary>
        event Action CreateAnchoredObjectStarted;

        /// <summary>
        /// Event fired when the Cloud Provider has located an SpatialPersistence from searching, may occur several times during a single search as they are located by the device.
        /// </summary>
        event Action AnchorLocated;

        /// <summary>
        /// Event fired when the Cloud Provider has deleted an SpatialPersistence. 
        /// </summary>
        event Action<string> AnchorDeleted;

        #endregion Internal Cloud Anchor Provider Events

        #region Spatial Persistence Service Events

        /// <summary>
        /// The Spatial Persistence provider reports that creation of the Cloud Anchor failed
        /// </summary>
        event Action CreateAnchoredObjectFailed;

        /// <summary>
        /// The Spatial Persistence provider reports that creation of the Cloud Anchor succeeded
        /// </summary>
        event Action<string, GameObject> CreateAnchoredObjectSucceeded;

        /// <summary>
        /// Status message whilst the Spatial Persistence service is localising the Cloud Anchor in place, continues until complete or a failure occurs
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
        /// Location request to Spatial Persistence service successful and a localised Cloud Anchor was found and cached.
        /// </summary>
        event Action<string> CloudAnchorLocated;

        #endregion Spatial Persistence Service Events
    }
}
