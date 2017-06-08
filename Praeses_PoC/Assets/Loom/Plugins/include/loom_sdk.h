//
// Copyright (C) 2017 Loomai, Inc.
// All rights reserved.
//
// Use of this software is subject to the terms of the Loomai license agreement
// provided at the time of installation or download, or which otherwise accompanies
// this software in either electronic or hard copy form.
//
//

// Loom SDK - C++ Header

#ifndef __LOOM_SDK_H__
#define __LOOM_SDK_H__

#if _WIN32
#define DLLEXPORT __declspec(dllexport)
#else
#define DLLEXPORT
#endif

#include <string>
#include <vector>

typedef enum {
    LoomReturnSuccess = 0,
    LoomReturnFailure = -1,
} LoomReturn;

//
// The C++ SDK is only using standard C++ type in its argument methods.
// All "get" methods taking non-const pointers will fill up the data assuming the pointers
//  got properly allocated by the caller.
//

class LoomImpl;
class DLLEXPORT Loom {
public:
    Loom();
    ~Loom();
    
    // Set the token to use for licensing validation. A rig can not be loaded unless this is set
    void setAvatarToken(const std::string& token);

    // Load up in memory the avatars from the sdk
    LoomReturn loadAvatar(const std::string& srcName, const std::string& targetName, bool activateBehaviors = true);
    
    // The internal engine works in right handed by default, some clients like Unity may need to revert this
    LoomReturn setLeftHandedSpace(bool state = true);
    
    // Provide geom data with a 1-1 correspondance between vertices and uv tables.
    //  Note that this will make getMeshVertexCount == getMeshUvCount and getMeshFaces == getMeshFaceUvs
    LoomReturn unweldMesh(const std::string& meshName);

    // Initiate the rigging process, before to set the current control values
    LoomReturn rigStart(const std::string& avatar);

    // Set the control values
    LoomReturn setControl(const std::string& avatar, const std::string& control, double weight);
    
    // Control predefined behaviors / animations
    LoomReturn setBehaviorActive(const std::string& avatar, const std::string& name, bool isActive = true);
    LoomReturn setBehaviorPlayOnce(const std::string& avatar, const std::string& name);
    
    // Add constraints values
    LoomReturn setTrackTargetTranslation(const std::string& avatar, float x, float y, float z);
    
    // Evaluate and apply the rig
    LoomReturn rigApplyAtTime(const std::string& avatar, double time);
    
    // Apply solved data returned by the server
    LoomReturn applySolvedLEyeTranslation(const std::string& avatar, float x, float y, float z);
    LoomReturn applySolvedREyeTranslation(const std::string& avatar, float x, float y, float z);
    LoomReturn applySolvedData(const std::string& avatar, const std::string& dataPath, unsigned char *data, int dataSize);
    LoomReturn applySolvedData(const std::string& avatar, const std::string& dataPath, const std::string& data);

    // Retrieve all geometry information to synchronize with the client rendenrer
    int getMeshVertexCount(const std::string& meshName);
    int getMeshUvCount(const std::string& meshName);
    int getMeshFaceCount(const std::string& meshName);
    LoomReturn getMeshUvs(const std::string& meshName, float *uvs);
    LoomReturn getMeshFaces(const std::string& meshName, int *faces);
    LoomReturn getMeshFaceUvs(const std::string& meshName, int *faceUvs);
    LoomReturn getReferenceMeshVertices(const std::string& meshName, float *vtx);
    LoomReturn getRenderMeshVertices(const std::string& meshName, float *vertices);
    LoomReturn getRenderMeshNormals(const std::string& meshName, float *normals);
    LoomReturn getRenderMeshTangentsV4(const std::string& meshName, float *tangents);

    // Procedural geometry assignement
    LoomReturn setReferenceMesh(const std::string& meshName, int vtxCount, float *vtx, int uvCount, float *uvs, int faceCount, int *faces, int faceUvCount, int *faceUvs);
    LoomReturn setMeshDeformer(const std::string& avatar, const std::string& geom, const std::string& srcGeom, float maxBoneDistance);
    LoomReturn setMeshSkinningFactor(const std::string& avatar, const std::string& geom, float factor);
    LoomReturn registerMesh(const std::string& avatar, const std::string& geom); // Make sure mesh exists in the avatar
    LoomReturn retargetMesh(const std::string& avatar, const std::string& geom); // Wrap deform using parent's initial-to-solved geometry
    
    // Node-level control in case the user needs to manage independent objects in the scene
    LoomReturn addMesh(const std::string& avatar, const std::string& name);
    LoomReturn removeNode(const std::string& name);

    // Query specific RIG Node Transformation info
    LoomReturn getNodeGlobalMatrix(const std::string& nodeName, float *matrix4x4);

    // Global evaluation of the whole scene
    void update(double time);
    
    // General information query
    LoomReturn getControls(const std::string& avatar, std::vector<std::string>& controlNames, std::vector<std::string>& controlDescriptions);
    bool hasControl(const std::string& avatar, const std::string& control);
    float getControlWeight(const std::string& avatar, const std::string& control);
    LoomReturn getBehaviors(const std::string& avatar, std::vector<std::string>& behaviorNames);

    // Performance control
    // level is set to 0 by default (high) and will auto-adjust based on other parameters
    LoomReturn setQualityLevel(const std::string& avatar, int level = 0, float fpsTarget = 30, int levelHighest = 0, int levelLowest = 4);
    
    // Export
    LoomReturn exportObj(const std::string& meshName, std::string& objAscii);
    
    // Logging facilities and debugging / profiling
    void logDebug(const std::string& message);
    void logInfo(const std::string& message);
    void logWarn(const std::string& message);
    void logErr(const std::string& message);
    void setLogCache(bool state = true);
    void setLogLevel(int level); // 2 -> info, 3 -> warning, 4 -> errors
    const std::string& flushLog(); // Query and clear the latest logs from the SDK

protected:
    LoomImpl *impl;
};

#endif
