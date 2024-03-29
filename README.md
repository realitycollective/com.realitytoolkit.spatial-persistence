# Reality Toolkit - Spatial Persistence Module

The Spatial Persistence Service package for building anchored solutions, built upon the Reality Collective [Service Framework](https://github.com/realitycollective/com.realitycollective.service-framework).

[![openupm](https://img.shields.io/npm/v/com.realitytoolkit.core?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.realitytoolkit.core/) [![Discord](https://img.shields.io/discord/597064584980987924.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/hF7TtRCFmB)
[![Publish main branch and increment version](https://github.com/realitycollective/com.realitytoolkit.core/actions/workflows/main-publish.yml/badge.svg)](https://github.com/realitycollective/com.realitytoolkit.core/actions/workflows/main-publish.yml)
[![Publish development branch on Merge](https://github.com/realitycollective/com.realitytoolkit.core/actions/workflows/development-publish.yml/badge.svg)](https://github.com/realitycollective/com.realitytoolkit.core/actions/workflows/development-publish.yml)
[![Build and test UPM packages for platforms, all branches except main](https://github.com/realitycollective/com.realitytoolkit.core/actions/workflows/development-buildandtestupmrelease.yml/badge.svg)](https://github.com/realitycollective/com.realitytoolkit.core/actions/workflows/development-buildandtestupmrelease.yml)

## Installation

Make sure to always use the same source for all toolkit modules. Avoid using different installation sources within the same project. We provide the following ways to install Reality Toolkit modules:

### Method 1: Using Package Manager for git users

1. Open the Package Manager using the Window menu -> Package Manager

2. Inside the Package Manager, click on the "+" button on the top left and select "Add package from git URL..."

3. Input the following URL: https://github.com/realitycollective/com.realitytoolkit.core.git and click "Add".

### Method 2: OpenUPM

```text
    openupm add com.realitytoolkit.spatial-persistence
```

## What's included?

- Spatial Persistence system manager

This provices the basic service for operating spatially aware systems such as cloud based point clouds and image tracking solutions.

The Reality Collective currently provides to "Modules" to provide spatial persistence tracking:

- [AR Foundation Image Tracking support](https://github.com/realitycollective/com.realitytoolkit.spatial-persistence.arfoundation)
  Enables registering and identification of Image Tracked targets using ARCOre/ARKit through a managed interface.
- [Azure ASA Anchors](https://github.com/realitycollective/com.realitytoolkit.spatial-persistence.asa)
  A cloud based point cloud location provider, which scans spaces for recognition and then enabled point cloud identification of a Cloud Anchor

Many other options are possible through the addition of tracking modules under a common "Spatial Tracking" API

## Requirements

- [RealityCollective.ServiceFramework](https://github.com/realitycollective/com.realitycollective.service-framework)
- [Unity 2021.3 and above](https://unity.com/)
- Modules may have their own requirements, e.g. ARFoundaiton or Azure Spatial Anchors dependencies.  CHeck each module for details

## Getting Started

- tbc
