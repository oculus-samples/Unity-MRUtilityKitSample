# Unity MRUK Sample

This sample project will help you navigate and understand the MR Utility Kit APIs through a collection of samples that. Feel free to delve into these APIs, try them out, and discover creative ways to apply them in your projects.

The [Oculus License](./LICENSE) applies to the samples.

This project was built using the [Unity engine](https://unity.com/download).

## Mixed Reality Utility Kit Overview

MR Utility Kit provides a rich set of utilities and tools on top of Scene API to perform common operations when building spatially-aware apps. This makes it easier to program against the physical world, and allows developers to focus on what makes their app unique.

The utilities provided broadly fall into 3 categories: Scene Queries, Graphical Helpers, Development Tools.

More information on MR Utility Kit can be found on our [developer website](https://developers.meta.com/horizon/documentation/unity/unity-mr-utility-kit-overview/).


## Project Structure

- [MRUKBase](./Assets/MRUKSamples/Basic): A basic scene showing the core functionality of MR Utility Kit leveraging the EffectMesh prefab to visualize the scene data.
- [Floor Zone](./Assets/MRUKSamples/FloorZone): A scene showing how to find a floor zone unobstructed by other scene anchors.
- [Multi Spawn](./Assets/MRUKSamples/MultiSpawn): A scene showing how to spawn objects in different locations within the room.
- [Nav Mesh](./Assets/MRUKSamples/NavMesh): A scene showing how to create a nav mesh for navigation leveraging the scene data.
- [Virtual Home](./Assets/MRUKSamples/VirtualHome): A scene showing how to reskin a room using the prefab spawner functionality.
- [Passthrough Relighting](./Assets/MRUKSamples/PassthroughRelighting): A scene showing the effect of virtual shadows and highlights on scene objects.
- [Scene Decorator](./Assets/MRUKSamples/SceneDecorator): A scene with basic decorations and SceneDecorator to populate the environment.
- [Destructible Mesh](./Assets/MRUKSamples/DestructibleMesh): A scene showing how to spawn a global mesh that can be segmented, usually used to create destructible environments.
- [Environment Panel Placement](./Assets/MRUKSamples/EnvironmentPanelPlacement): A scene showing how to use EnvironmentRaycastManager to attach virtual panel to the physical environment.
- [Space Map](./Assets/MRUKSamples/SpaceMap): A scene with the SpaceMap prefab added. It creates a texture which represents the room with a color gradient according to the settings of the prefab.
- [Keyboard Tracking](./Assets/MRUKSamples/KeyboardTracker): A scene demonstrating generic keyboard detection and tracking.
- [Bouncing Ball](./Assets/MRUKSamples/BouncingBall): A scene showing virtual balls interacting with the physical environment.
- [QR Code Detection](./Assets/MRUKSamples/QRCodeDetection): A scene demonstrating QR code detection.
- [HiFi Scene](./Assets/MRUKSamples/HiFiScene): A scene showing multiple floors and ceilings, slanted ceilings, inner walls.

More information on all samples can be found on our [developer website](https://developers.meta.com/horizon/documentation/unity/unity-mr-utility-kit-samples).

## Getting The Code

Clone this repo using the "Code" button above, or this command:
```sh
git clone https://github.com/oculus-samples/Unity-MRUtilityKitSample.git
```

## How to run the project in Unity

1. Make sure you're using  *Unity 2022.3.15f1* or newer.
2. In the Project window, navigate to [Assets/MRUKSamples/](Assets/MRUKSamples).
3. Click on individual scenes.
4. Click **Play** button to explore scene functionality in Unity.

## How to test on device

1. Navigate to **File** > **Build Settings**.
2. Select the sample scene that you want to test on device.
3. Build an apk.
4. Navigate to build destination folder and copy the APK to your device using [Meta Quest Developer Hub](https://developer.oculus.com/documentation/unity/ts-mqdh-deploy-build/).

## SDK Dependencies

All Meta SDKs can be found in the [Unity Asset Store](https://assetstore.unity.com/publishers/25353).
This project depends on SDKs defined in the [Packages/manifest.json](./Packages/manifest.json):

* [Meta XR Core SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-core-sdk-269169)
* [Meta XR MR Utility Kit SDK](https://assetstore.unity.com/packages/tools/integration/meta-mr-utility-kit-272450)

## Integrate Samples to your own project

1. Make sure your project uses the same SDK version
2. Move the samples to your project
   <details>
      <summary><b>Copy Samples directory</b></summary>

      + Copy [Assets/MRUKSamples](./Assets/MRUKSamples) directory to your own project
    </details>
    <details>
      <summary><b>Create UnityPackage and Import it</b></summary>

      1. Open Unity-MRUtilityKitSample project in Unity
      2. Right-click on [Assets/MRUKSamples](./Assets/MRUKSamples) and select <i>Export Package...</i>
      3. Save package in an easy location to retrieve
      4. Open your own project (where you want the samples to be added)
      5. Click on <i>Assets->Import Package->Custom Package...</i> from the menu bar
      6. Find the package we saved in step 3 and click <i>Open</i>
    </details>

## Licenses

The Unity-MRUtilityKitSample project is licensed under [MIT LICENSE](./LICENSE).

## Agent-ready development

This sample is configured for AI coding agents.

For the best experience in VS Code or Cursor, install the Meta Horizon extension:

https://marketplace.visualstudio.com/items?itemName=meta.meta-vr-dev

This repository also includes agent instructions and MCP configuration for Meta Quest Agentic Tools:

https://github.com/meta-quest/agentic-tools

Suggested first prompt:

```text
Read AGENTS.md, detect what type of Meta Quest sample this is, enable the hzdb MCP server if available, and explain how to build, run, and debug this sample on a Quest device.
```
