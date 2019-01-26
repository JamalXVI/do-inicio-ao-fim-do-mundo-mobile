using Comum.Controladores;
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

        private float ContadorTempoChao = 0;
        private float ContadorRedutor = 0;
        private float TempoReduzido = 0f;
        private float TempoRedutor = 0f;
        private bool PararDeContarRedutor = false;
        private int IdDoTileParaDesaparecer = 0;

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
            var posicaoMin = transforms.Select(tr => tr.position.x - tr.localScale.x * tr.GetComponent<SpriteRenderer>().size.x).Min();
            var posicaoMax = transforms.Select(tr => tr.position.x).Max();
            float tamanhoDaImagem = GetComponentsInChildren<SpriteRenderer>().Select(sp => sp.size.x).Sum();
            float escala = GetComponentsInChildren<Transform>().Select(t => t.localScale.x).Max();
            Propriedades = new Propriedades(
                posicaoMin: posicaoMin,
                posicaoMax: posicaoMax,
                transforms: transforms.ToList(),
                escala: escala);
        }

        private void Start()
        {
            TempoReduzido = Gerenciador.Instancia.ChaoConfig.TempoParaDesaparecer;
            TempoRedutor = Gerenciador.Instancia.ChaoConfig.RedutorDificuldade;
        }



        void Update()
        {
            CalcularParaOTempoDoChaoDesaparecer();
            var posicaoMax = this.Propriedades.Transforms.Select(tr => tr.position.x + tr.localScale.x * tr.GetComponent<SpriteRenderer>().size.x).Max();
            for (var index = 0; index < this.Propriedades.Transforms.Count; index++)
            {
                var tr = this.Propriedades.Transforms.ElementAt(index);
                Vector3 pos = Posicoes.ElementAt(index);
                if (tr.position.x < this.Propriedades.PosicaoMin * this.Propriedades.Escala)
                {
                    if(this.Propriedades.Escondido[index] && !this.Propriedades.Escondeu[index])
                    {
                        var verficarAtras = false;
                        var verificarAFrente = false;
                        bool pularDesativar = false;
                        for (int j = 1; j < Gerenciador.Instancia.ChaoConfig.LimiteVisinhos+1; j++)
                        {
                            if (index-j > 0 && this.Propriedades.Escondeu[index - j])
                            {
                                if (verficarAtras)
                                {
                                    pularDesativar = true;
                                    goto continuarDepoisDoFor;
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
                                    goto continuarDepoisDoFor;
                                }
                                else
                                {
                                    verificarAFrente = true;
                                }
                            }
                        }
                        continuarDepoisDoFor:
                        if (!pularDesativar)
                        {
                            tr.GetComponent<SpriteRenderer>().enabled = false;
                            tr.GetComponent<BoxCollider2D>().enabled = false;
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
                        tr.GetComponent<SpriteRenderer>().enabled = true;
                        tr.GetComponent<BoxCollider2D>().enabled = true;
                        this.Propriedades.Escondido[index] = false;
                        this.Propriedades.Escondeu[index] = false;
                    }
                    pos = new Vector3(posicaoMax, pos.y, pos.z);
                }
                pos += Vector3.left * Time.deltaTime * this.Velocidade;
                tr.position = pos;
                Posicoes[index] = pos;
            };
        }

        private void CalcularParaOTempoDoChaoDesaparecer()
        {
            CalcularTempoRedutor();
            ContadorTempoChao += Time.deltaTime;
            if (ContadorTempoChao >= TempoReduzido)
            {
                ContadorTempoChao = 0;
                DefinirChaoParaEsconder();
            }
        }

        private void DefinirChaoParaEsconder()
        {
            IdDoTileParaDesaparecer = Random.Range(0, this.Propriedades.Transforms.Count);
            this.Propriedades.Escondido[IdDoTileParaDesaparecer] = true;
        }

        private void CalcularTempoRedutor()
        {
            if (!PararDeContarRedutor)
            {
                ContadorRedutor += Time.deltaTime;
                if (ContadorRedutor >= TempoRedutor)
                {
                    ContadorRedutor = 0;
                    TempoRedutor -= TempoRedutor * Gerenciador.Instancia.ChaoConfig.MultiplicadoDificuldade * ((int)Gerenciador.Instancia.Dificuldade + 1);
                    TempoReduzido -= Gerenciador.Instancia.ChaoConfig.UnidadeTempo;
                    if (Gerenciador.Instancia.ChaoConfig.TempoLimite >= TempoReduzido)
                    {
                        TempoReduzido = Gerenciador.Instancia.ChaoConfig.TempoLimite;
                        PararDeContarRedutor = true;
                    }
                    Debug.Log(TempoReduzido);
                }

            }
        }
    }
}