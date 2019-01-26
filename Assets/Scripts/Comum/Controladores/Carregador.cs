using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Comum.Controladores
{
    public class Carregador : MonoBehaviour
    {
        public GameObject gerenciador;
        private void Awake()
        {
            if (Gerenciador.Instancia == null)
            {
                Instantiate(gerenciador);
            }
        }

    }
}