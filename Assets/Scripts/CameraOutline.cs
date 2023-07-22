using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraOutline : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (this.enabled)
		{
			Color c = Gizmos.color;
			Gizmos.color = Color.yellow;
			Camera cam = this.GetComponent<Camera>();
			if (cam.orthographic)
			{
				Vector3 center = Vector3.back * (cam.nearClipPlane + cam.farClipPlane) * 0.5f;
				Vector3 size = new Vector3(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2, cam.farClipPlane - cam.nearClipPlane);

				Matrix4x4 m = Gizmos.matrix;
				Gizmos.matrix = cam.cameraToWorldMatrix;
				Gizmos.DrawWireCube(center, size);
				Gizmos.matrix = m;
			}
			else
			{
				Matrix4x4 m = Gizmos.matrix;
				Matrix4x4 matrixCam = cam.cameraToWorldMatrix;
				Matrix4x4 nagtiveZ = Matrix4x4.identity;
				nagtiveZ.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 0), new Vector3(1, 1, -1));
				Gizmos.matrix = matrixCam * nagtiveZ;
				// center是平截头的顶端，即摄像机的位置。相对于自己是zero.
				Vector3 center = Vector3.zero;
				Gizmos.DrawFrustum(center, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
				Gizmos.matrix = m;
			}
			Gizmos.color = c;
		}
	}
}
