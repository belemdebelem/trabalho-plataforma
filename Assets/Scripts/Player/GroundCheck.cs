using UnityEngine;

/// <summary>
/// Detecta se o personagem esta apoiado em uma superficie valida.
/// O SphereCast e filtrado pela mascara de chao e pela normal do contato,
/// evitando falsos positivos ao encostar lateralmente nas plataformas.
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [Tooltip("Camadas consideradas como chao. A camada 2 (Ignore Raycast) e usada pelo jogador.")]
    public LayerMask groundMask = ~(1 << 2);

    [Tooltip("Raio do SphereCast usado na verificacao.")]
    public float castRadius = 0.35f;

    [Tooltip("Distancia extra de verificacao abaixo dos pes.")]
    public float castDistance = 0.25f;

    public bool IsGrounded
    {
        get
        {
            Vector3 origin = transform.position + Vector3.up * (castRadius + 0.05f);

            if (Physics.SphereCast(
                    origin,
                    castRadius,
                    Vector3.down,
                    out RaycastHit hit,
                    castDistance + 0.05f,
                    groundMask,
                    QueryTriggerInteraction.Ignore))
            {
                return hit.normal.y >= 0.5f;
            }

            return false;
        }
    }
}
