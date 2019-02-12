using Comum.Controladores;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private float contadorTempoChao = 0f;
        private float contadorRedutor = 0f;
        private float tempoReduzido = 0f;
        private float tempoRedutor = 0f;
        private bool pararDeContarRedutor = false;
        private int idDoTileParaDesaparecer = 0;
        private float posicaoMax = 0f;
        private float novaPosicaoMax = 0f;
        private float tamanhoQuadrado = 0f;

        public float Velocidade
        {
            get
            {
                return _velocidade;
            }
        }


        private void Awake()
        {
            var transforms = (from tr in GetComponentsInChildren<Transform>() where tr.parent == transform select tr).ToList();
            Posicoes = (from tr in transforms select tr.position).ToList();
            tamanhoQuadrado = transforms.Select(tr => tr.localScale.x * tr.GetComponent<SpriteRenderer>().size.x).Max();
            var posicaoMin = transforms.Select(tr => tr.position.x - tamanhoQuadrado).Min();
            var posicaoMax = transforms.Select(tr => tr.position.x).Max();
            float tamanhoDaImagem = GetComponentsInChildren<SpriteRenderer>().Select(sp => sp.size.x).Sum();
            float escala = GetComponentsInChildren<Transform>().Select(t => t.localScale.x).Max();
            var sprites = from tr in transforms select tr.GetComponent<SpriteRenderer>();
            var boxColliders = (from tr in transforms select tr.GetComponent<BoxCollider2D>()).ToList();
            for(int i = 0; i < boxColliders.Count; i++)
            {
                for (int j = i+1; j < boxColliders.Count; j++)
                {
                    Physics2D.IgnoreCollision(boxColliders[i], boxColliders[j]);
                }
            }
            Propriedades = new Propriedades(
                posicaoMin: posicaoMin,
                posicaoMax: posicaoMax,
                transforms: transforms,
                escala: escala,
                sprites: sprites.ToList(),
                boxColliders: boxColliders);
        }

        private void Start()
        {
            tempoReduzido = Gerenciador.Instancia.ChaoConfig.TempoParaDesaparecer;
            tempoRedutor = Gerenciador.Instancia.ChaoConfig.RedutorDificuldade;
        }



        void FixedUpdate()
        {
            CalcularParaOTempoDoChaoDesaparecer();
            novaPosicaoMax = float.MinValue;
            var posicaoMudar = Vector3.left * Time.deltaTime * this.Velocidade;
            for (var index = 0; index < this.Propriedades.Transforms.Count; index++)
            {
                var tr = this.Propriedades.Transforms.ElementAt(index);
                Vector3 pos = Posicoes.ElementAt(index);
                if (tr.position.x < this.Propriedades.PosicaoMin * this.Propriedades.Escala)
                {
                    EscoderChao(index);
                    pos.x = posicaoMax;
                }
                pos += posicaoMudar;
                if (pos.x + tamanhoQuadrado > novaPosicaoMax)
                {
                    novaPosicaoMax = pos.x + tamanhoQuadrado;
                }
                tr.position = pos;
                Posicoes[index] = pos;
            };
            posicaoMax = novaPosicaoMax;
        }


        private void CalcularParaOTempoDoChaoDesaparecer()
        {
            CalcularTempoRedutor();
            contadorTempoChao += Time.deltaTime;
            if (contadorTempoChao >= tempoReduzido)
            {
                contadorTempoChao = 0;
                DefinirChaoParaEsconder();
            }
        }

        private void DefinirChaoParaEsconder()
        {
            idDoTileParaDesaparecer = Random.Range(0, this.Propriedades.Transforms.Count);
            this.Propriedades.Escondido[idDoTileParaDesaparecer] = true;
        }

        private void CalcularTempoRedutor()
        {
            if (!pararDeContarRedutor)
            {
                contadorRedutor += Time.deltaTime;
                if (contadorRedutor >= tempoRedutor)
                {
                    contadorRedutor = 0;
                    tempoRedutor -= tempoRedutor * Gerenciador.Instancia.ChaoConfig.MultiplicadoDificuldade * ((int)Gerenciador.Instancia.Dificuldade + 1);
                    tempoReduzido -= Gerenciador.Instancia.ChaoConfig.UnidadeTempo;
                    if (Gerenciador.Instancia.ChaoConfig.TempoLimite >= tempoReduzido)
                    {
                        tempoReduzido = Gerenciador.Instancia.ChaoConfig.TempoLimite;
                        pararDeContarRedutor = true;
                    }
                }

            }
        }
        private void EscoderChao(int index)
        {
            if (this.Propriedades.Escondido[index] && !this.Propriedades.Escondeu[index])
            {
                var verficarAtras = false;
                var verificarAFrente = false;
                bool pularDesativar = false;
                for (int j = 1; j < Gerenciador.Instancia.ChaoConfig.LimiteVisinhos + 1; j++)
                {
                    if (index - j > 0 && this.Propriedades.Escondeu[index - j])
                    {
                        if (verficarAtras)
                        {
                            pularDesativar = true;
                            break;
                        }
                        else
                        {
                            verficarAtras = true;
                        }
                    }
                    if (index + j < this.Propriedades.Transforms.Count && this.Propriedades.Escondeu[index + j])
                    {
                        if (verificarAFrente)
                        {
                            pularDesativar = true;
                            break;
                        }
                        else
                        {
                            verificarAFrente = true;
                        }
                    }
                }
                if (!pularDesativar)
                {
                    this.Propriedades.Sprites[index].enabled = false;
                    this.Propriedades.BoxColliders[index].enabled = false;
                    this.Propriedades.Escondeu[index] = true;
                }
                else
                {
                    this.Propriedades.Escondido[index] = false;
                    DefinirChaoParaEsconder();
                }
            }
            else if (this.Propriedades.Escondido[index] && this.Propriedades.Escondeu[index])
            {
                this.Propriedades.Sprites[index].enabled = true;
                this.Propriedades.BoxColliders[index].enabled = true;
                this.Propriedades.Escondido[index] = false;
                this.Propriedades.Escondeu[index] = false;
            }

        }
        #region Coroutines
        #endregion
    }
}