using System.Collections.Generic;
using UnityEngine;

namespace Comum.Chao
{
    public class Propriedades
    {

        public float PosicaoMin { get; }

        public float PosicaoMax { get; }

        public List<Transform> Transforms { get; }
        public List<bool> Escondido { get; }
        public List<bool> Escondeu { get; }

        public float Escala { get; }

        public Propriedades(float posicaoMin, float posicaoMax, List<Transform> transforms, float escala)
        {
            PosicaoMin = posicaoMin;
            PosicaoMax = posicaoMax;
            Transforms = transforms;
            Escondido = new List<bool>(transforms.Count).RepetirPorPadrao();
            Escondeu = new List<bool>(transforms.Count).RepetirPorPadrao();
            Escala = escala;

        }
    }
}