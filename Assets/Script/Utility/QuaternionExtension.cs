using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionExtension
{
    public static Vector3 GetPitchYawRollRad(this Quaternion rotation)
    {
        float roll = Mathf.Atan2(
            2 * rotation.y * rotation.w - 2 * rotation.x * rotation.z,
            1 - 2 * rotation.y * rotation.y - 2 * rotation.z * rotation.z
        );
        float pitch = Mathf.Atan2(
            2 * rotation.x * rotation.w - 2 * rotation.y * rotation.z,
            1 - 2 * rotation.x * rotation.x - 2 * rotation.z * rotation.z
        );
        float yaw = Mathf.Asin(2 * rotation.x * rotation.y + 2 * rotation.z * rotation.w);

        return new Vector3(pitch, yaw, roll);
    }

    public static Vector3 GetPitchYawRollDeg(this Quaternion rotation)
    {
        Vector3 radResult = GetPitchYawRollRad(rotation);
        return new Vector3(
            radResult.x * Mathf.Rad2Deg,
            radResult.y * Mathf.Rad2Deg,
            radResult.z * Mathf.Rad2Deg
        );
    }
}
