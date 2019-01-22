using UnityEngine;

namespace Personagem
{
    public class Socolinha : PersonagemBase
    {
        protected override void AnimarPulo()
        {
            Animator.SetTrigger("Jumping");
        }

        protected override void FinalizarAnimacaoPulo()
        {
            Animator.SetTrigger("Falling");
        }
    }
}
