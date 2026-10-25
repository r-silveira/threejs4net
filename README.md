***IMPORTANT***: This repo was created to provide an updated version of the original codebase (Three.cs) which was abandoned (https://github.com/lathoub/three.cs) by it's original author (https://github.com/lathoub).

=======

C# implementation of the Three.js data structures and math.

The aim of the Three.cs library is to be as close as possible to Three.js project, so that applications are as easy to port from one environment to another - taking into account the different capabilities of the programming languages. 

Note: Some JavaScript classes are created 'on-the-fly' during the execution of the renderer. C# does not easily allow dynamic creation of classes, these classes are pre-baked.

The library is being updated to reflect ThreeJS capabilities as of R116. After that we will start moving it to the most recent ThreeJS release.

Note 1: The library is far from being finished, with a handful of fully working examples, so still needs a lot of work.
Note 2: At this moment, we are focused on updating all ThreeJS classes and methods and not so much on the rendering part of the process. 

## Data structures only (no rendering)

This branch keeps only the three.js data structures and math (Vector3, Matrix4, Box3, BufferGeometry,
BufferAttribute, Object3D, Mesh, materials, cameras, lights, geometries, curves...). The OpenGL/WebGL
renderer, the GLSL shaders, render targets, the software projector and the WinForms/OpenTK demo were
removed. The library has no Windows or OpenGL dependency and targets plain `net6.0`.

## Usage

Reference `ThreeJs4Net/ThreeJs4Net.csproj` from your project. Tests are in `ThreeJs4Net.Tests` (`dotnet test`).
