using UnityEngine;

namespace Comum.Chao
{
    public class Propriedades
    {
        private readonly Vector3 _posicaoInicial;
        public Vector3 PosicaoInicial
        {
            get
            {
                return this._posicaoInicial;
            }
        }
        private readonly float _tamanhoImagemReal;
        public float TamanhoImagemReal
        {
            get
            {
                return this._tamanhoImagemReal;
            }
        }
        public Propriedades(Vector3 posicaoInicial, float tamanhoImagemReal)
        {
            this._posicaoInicial = posicaoInicial;
            this._tamanhoImagemReal = tamanhoImagemReal;
        }
    }
}