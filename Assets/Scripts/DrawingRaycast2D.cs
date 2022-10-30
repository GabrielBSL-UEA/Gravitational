using UnityEngine;

public class DrawingRaycast2D : MonoBehaviour
{
    public static Collider2D BoxCast(Vector2 origin, Vector2 size, LayerMask layer, Color noHitColor, Color hitColor, float duration = .02f)
    {
        //Instancia uma caixa dada as medidas fornecidas que serão usadas para verificar colisões com a layer passada pelo jogador
        var hit = Physics2D.OverlapBox(
            origin,
            size,
            0,
            layer);

        //Seta a cor da caixa, dependendo de se a caixa registrou alguma colisão ou não
        Color boxColor = hit ? hitColor : noHitColor;

        //Desenha a caixa no editor da Unity
        Debug.DrawRay(origin - (size / 2), Vector2.up * size.y, boxColor);
        Debug.DrawRay(origin - (size / 2), Vector2.right * size.x, boxColor);
        Debug.DrawRay(origin + (size / 2), Vector2.left * size.x, boxColor);
        Debug.DrawRay(origin + (size / 2), Vector2.down * size.y, boxColor);

        return hit;
    }

    public static Collider2D[] BoxCastAll(Vector2 origin, Vector2 size, LayerMask layer, Color noHitColor, Color hitColor, float duration = .02f)
    {
        //Instancia uma caixa dada as medidas fornecidas que serão usadas para verificar colisões com a layer passada pelo jogador
        var hit = Physics2D.OverlapBoxAll(
            origin,
            size,
            0,
            layer);

        //Seta a cor da caixa, dependendo de se a caixa registrou alguma colisão ou não
        Color boxColor = hit.Length > 0 ? hitColor : noHitColor;

        //Desenha a caixa no editor da Unity
        Debug.DrawRay(origin - (size / 2), Vector2.up * size.y, boxColor);
        Debug.DrawRay(origin - (size / 2), Vector2.right * size.x, boxColor);
        Debug.DrawRay(origin + (size / 2), Vector2.left * size.x, boxColor);
        Debug.DrawRay(origin + (size / 2), Vector2.down * size.y, boxColor);

        return hit;
    }
}

