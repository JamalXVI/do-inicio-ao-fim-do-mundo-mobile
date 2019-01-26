using UnityEngine;
using UnityEngine.Serialization;

namespace Comum.Controladores.Config
{
    public class ChaoConfig : MonoBehaviour
    {

        [FormerlySerializedAs("Tempo Desaparecer")]
        [Tooltip("O Tempo Para Desaparecer o Chão, ou seja a cada X segundos um quadrado do chão desaparece")]
        [SerializeField]
        private float _TempoParaDesaparecer;
        public float TempoParaDesaparecer
        {
            get
            {
                return _TempoParaDesaparecer;
            }
        }

        [FormerlySerializedAs("Multiplicador Dificuldade")]
        [Tooltip("O Fator que irá multiplicar o Redutor de Dificuldade, conforme a dificuldade escolhida. Por exemplo: Se o multiplicador da dificuldade for 0.75 a cada aumento" +
            " de dificuldade diminui em 25% (100% - 75%) o tempo de Redutor de dificuldade")]
        [SerializeField]
        private float _MultiplicadoDificuldade;
        public float MultiplicadoDificuldade
        {
            get
            {
                return _MultiplicadoDificuldade;
            }
        }
        [FormerlySerializedAs("Redutor Dificuldade")]
        [Tooltip("O Tempo que irá começar a reduzir 1 segundo do tempo para o Chão desaparecer. Ou seja a Cada Y segundos diminui K segundo dos " +
            "X segundos iniciais para desaparecer o chão. Esse parâmetro é dependente da dificuldade")]
        [SerializeField]
        private float _RedutorDificuldade;
        public float RedutorDificuldade
        {
            get
            {
                return _RedutorDificuldade;
            }
        }
        [FormerlySerializedAs("Tempo Limite")]
        [Tooltip("O tempo minímo que não pode ser mais reduzido. Ou seja: Se o tempo for reduzido para Z segundos, ignorar o Redutos de Dificuldade")]
        [SerializeField]
        private float _TempoLimite;
        public float TempoLimite
        {
            get
            {
                return _TempoLimite;
            }
        }
        [FormerlySerializedAs("Limite Visinhos")]
        [Tooltip("O Número de Chão Visinhos que podem estar invisíveis")]
        [SerializeField]
        private float _LimiteVisinhos;
        public float LimiteVisinhos
        {
            get
            {
                return _LimiteVisinhos;
            }
        }

        [FormerlySerializedAs("Unidade Tempo")]
        [Tooltip("O Tempo (segundos) que será reduzido do Tempo para desaparecer. Ou seja: Quando estourar o redutor limite, ele irá reduzir K unidades, sendo K esta unidade")]
        [SerializeField]
        private float _UnidadeTempo;
        public float UnidadeTempo
        {
            get
            {
                return _UnidadeTempo;
            }
        }
    }
}