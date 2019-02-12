using UnityEngine;

namespace Comum
{
    public sealed class Constantes
    {
        public static readonly short[] MODIFICADORES_DIFICULDADE = new short[] { 1, 2, 3 };
        public static readonly EnumDificuldade DIFICULDADE_PADRAO  = EnumDificuldade.FACIL;

        public static bool GetActionButton()
        {
            return Input.GetButtonDown("Fire1") || Input.touchCount > 0;
        }
    }
}