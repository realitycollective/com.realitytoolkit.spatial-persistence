<!-- Offline documentation -->

# XRTK Anchor Service

The XRTK ANchor service is a container for managing and connecting to various Spatial Anchoring services provided by several vendors, namely:

* Azure Spatial Anchors
* Google Cloud Anchors

(ARFoundation anchors not included as they don't have any possibility of a cloud backend)

Each implementation will be available in a separate package that can be installed and then configured as a provider of cloud Anchors, this is to reduce the amount of dependencies any project needs to install, to ensure you only have what you need.

> Please check the notes in each provider package to include the relevant additional packages you will need.