using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Comum.Chao
{
    public class MovimentarChao : MonoBehaviour
    {
        [FormerlySerializedAs("Velocidade")]
        [SerializeField]
        private float _velocidade = 0f;

        public Propriedades Propriedades { get; private set; }

        public List<Vector3> Posicoes { get; set; }

        public float Velocidade
        {
            get
            {
                return _velocidade;
            }
        }


        private void Awake()
        {
            var transforms = (from tr in GetComponentsInChildren<Transform>() where tr.parent == transform select tr);
            Posicoes = (from tr in transforms select tr.position).ToList();
            var posicaoMin = transforms.Select(tr => tr.position.x -  tr.GetComponentsInChildren<SpriteRenderer>().Select(sp => sp.transform.localScale.x * sp.size.x).Sum()).Min();
            var posicaoMax = transforms.Select(tr => tr.position.x).Max();
            float tamanhoDaImagem = GetComponentsInChildren<SpriteRenderer>().Select(sp => sp.size.x).Sum();
            float escala = GetComponentsInChildren<Transform>().Select(t => t.localScale.x).Max();
            Propriedades = new Propriedades(
                posicaoMin: posicaoMin,
                posicaoMax: posicaoMax,
                transforms: transforms.ToList(),
                escala: escala);
        }
        void Update()
        {
            var posicaoMax = this.Propriedades.Transforms.Select(tr => tr.position.x + tr.GetComponentsInChildren<SpriteRenderer>().Select(sp => sp.transform.localScale.x * sp.size.x).Sum()).Max();
            for (var index = 0; index < this.Propriedades.Transforms.Count; index++)
            {
                var tr = this.Propriedades.Transforms.ElementAt(index);
                Vector3 pos = Posicoes.ElementAt(index);
                if (tr.position.x < this.Propriedades.PosicaoMin * this.Propriedades.Escala)
                {
                    pos = new Vector3(posicaoMax, pos.y, pos.z);
                }
                pos += Vector3.left * Time.deltaTime * this.Velocidade;
                tr.position = pos;
                Posicoes[index] = pos;
            };
        }
    }
}