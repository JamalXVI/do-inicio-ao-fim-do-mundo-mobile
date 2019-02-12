using System;
using UnityEngine;

namespace Personagem
{

    public abstract class PersonagemBase : MonoBehaviour
    {

        [SerializeField]
        private float _forca = 60f;
        protected internal float Forca
        {
            get
            {
                return _forca;
            }
        }
        [SerializeField]
        private Transform _checagemChao;
        protected internal Transform ChecagemChao
        {
            get
            {
                return _checagemChao;
            }
        }

        public Rigidbody2D Corpo { get; set; }

        protected internal float AlturaMaxima { get; private set; }


        protected internal Vector3 PosicaoInicial { get; private set; }

        protected internal bool EmPulo { get; private set; }


        protected internal bool PosicaoInicialSetada { get; private set; }

        protected internal bool AcionouAnimacaoPulo { get; private set; }

        protected internal Animator Animator { get; private set; }
        // Update is called once per frame
        void Update()
        {
            RealizarAcoes();
        }

        private void RealizarAcoes()
        {
            if (!PosicaoInicialSetada)
            {
                return;
            }
            if (Input.GetButtonDown("Fire1") && !EmPulo)
            {
                Pular();
            }
        }

        protected internal void Awake()
        {
            Corpo = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            CalcularAlturaMaxima();
        }

        protected internal void Pular()
        {
            EmPulo = true;
            Corpo.velocity = Vector2.zero;
            Corpo.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
            AnimarPulo();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (!PosicaoInicialSetada)
            {
                PosicaoInicial = transform.position;
                PosicaoInicialSetada = true;
            }

        }

        protected internal void FixedUpdate()
        {
            //Debug.DrawLine(new Vector3(-100, AlturaMaxima, 0), new Vector3(100, AlturaMaxima, 0), Color.red);
            VerificarPosicoes();
        }

        private void VerificarPosicoes()
        {
            if (!PosicaoInicialSetada)
            {
                return;
            }
            if (!EmPulo)
            {
                return;
            }
            if (transform.position.y >= AlturaMaxima && !AcionouAnimacaoPulo)
            {
                AcionouAnimacaoPulo = true;
                FinalizarAnimacaoPulo();
            }
            if (Vector2.Distance(PosicaoInicial, transform.position) < 0.0006 && AcionouAnimacaoPulo)
            {
                AcionouAnimacaoPulo = false;
                EmPulo = false;
            }
        }
        protected internal void CalcularAlturaMaxima()
        {
            float g = Corpo.gravityScale * Physics2D.gravity.magnitude;
            float v0 = Forca / Corpo.mass; // Converte a força do pulo em Aceleração
            float tamanhoCaixa = ChecagemChao.GetComponentInChildren<SpriteRenderer>().size.y * ChecagemChao.transform.localScale.y / 2;
            AlturaMaxima = ChecagemChao.position.y - tamanhoCaixa + (v0 * v0) / (2 * g);
        }
        protected abstract void AnimarPulo();
        protected abstract void FinalizarAnimacaoPulo();

    }
}
