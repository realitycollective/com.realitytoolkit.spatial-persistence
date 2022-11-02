<!-- Offline documentation -->

# Reality Toolkit Spatial Persistence Service

The Reality Toolkit Spatial Persistence Service is a container for managing and connecting to various Spatial Anchoring services provided by several vendors, namely:

* [Azure Spatial Anchors](https://github.com/realitycollective/com.realitytoolkit.spatial-persistence.asa)
* Image Tracked Markers (tbc)

(ARFoundation anchors are not included as they do not support a cloud backend and are not transferable from device to device)

Each vendor implementation will be available in a separate package that can be installed and then configured as an Anchors provider, this is to reduce the amount of dependencies any project needs to install, to ensure you only have what you need.

> Please check the notes in each provider package to include the relevant additional packages you will need.