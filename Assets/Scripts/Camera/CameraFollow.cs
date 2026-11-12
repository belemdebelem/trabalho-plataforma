using UnityEngine;

/// <summary>
/// Acompanhamento lateral simples da camera ortografica.
/// Mantem o eixo Z fixo e suaviza a movimentacao nos eixos X e Y.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target;

    [Header("Configuracao")]
    public Vector3 offset = new Vector3(0f, 3f, -20f);
    public float smoothTime = 0.15f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
