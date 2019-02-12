using Comum.Controladores.Config;
using UnityEngine;

namespace Comum.Controladores
{
    public class Gerenciador : MonoBehaviour
    {
        public static Gerenciador Instancia = null;
        public EnumDificuldade Dificuldade = Constantes.DIFICULDADE_PADRAO;
        [SerializeField]
        private GameObject _ConfigChao;

        private ChaoConfig _ChaoConfig;
        public ChaoConfig ChaoConfig
        {
            get
            {
                return _ChaoConfig;
            }
        }
        private void Awake()
        {
            if (Instancia == null)
            {
                Instancia = this;
            }
            else if (Instancia != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_ConfigChao.gameObject);
            IniciarVariaveis();
        }

        private void IniciarVariaveis()
        {
            GameObject chaoConfig = Instantiate(_ConfigChao);
            _ChaoConfig = chaoConfig.GetComponent<ChaoConfig>();
        }


    }
}
