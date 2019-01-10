using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Comum.Chao
{
    public class MovimentarChao : MonoBehaviour
    {
        [FormerlySerializedAs("Velocidade")]
        [SerializeField]
        private float _velocidade;

        public Propriedades Propriedades { get; private set; }

        public float Velocidade
        {
            get
            {
                return _velocidade;
            }
        }


        private void Awake()
        {
            float tamanhoDaImagem = this.GetComponent<SpriteRenderer>().size.x;
            float escala = this.transform.localScale.x;
            this.Propriedades = new Propriedades(this.transform.position, tamanhoDaImagem * escala);
        }
        void Update()
        {
            float deslocamento = Mathf.Repeat(this.Velocidade * Time.time, this.Propriedades.TamanhoImagemReal);
            this.transform.position = this.Propriedades.PosicaoInicial + Vector3.left * deslocamento;
        }
    }

}