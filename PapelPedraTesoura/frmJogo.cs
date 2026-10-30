
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PapelPedraTesoura
{
    public partial class frmJogo : Form
    {
        // Sorteio do computador
        Random gerador = new Random();

        // Pontuação
        int vitoriasJogador = 0;
        int vitoriasComputador = 0;

        // Guarda a jogada do jogador
        int jogadaEscolhida;

        // Imagens originais
        Image imagemPapelOriginal;
        Image imagemPedraOriginal;
        Image imagemTesouraOriginal;

        // Imagens em preto e branco
        Bitmap papelCinza;
        Bitmap pedraCinza;
        Bitmap tesouraCinza;

        // Timer do pensamento do computador
        Timer timerPensamento = new Timer();

        // Construtor
        public frmJogo()
        {
            InitializeComponent();

            // Guarda as imagens originais
            imagemPapelOriginal = picMaoPapel.Image;
            imagemPedraOriginal = picMaoPedra.Image;
            imagemTesouraOriginal = picMaoTesoura.Image;

            // Cria as imagens em preto e branco
            papelCinza = PretoEBranco(imagemPapelOriginal);
            pedraCinza = PretoEBranco(imagemPedraOriginal);
            tesouraCinza = PretoEBranco(imagemTesouraOriginal);

            // Configura o Timer
            timerPensamento.Interval = 1500;
            timerPensamento.Tick += timerPensamento_Tick;

            // Eventos das imagens
            picMaoPapel.Click += picMaoPapel_Click;
            picMaoPedra.Click += picMaoPedra_Click;
            picMaoTesoura.Click += picMaoTesoura_Click;

            // Eventos dos botões
            btnLimparPlacar.Click += btnLimparPlacar_Click;
            btnNovaPartida.Click += btnNovaPartida_Click;
        }

        // Jogador escolheu PAPEL
        private void picMaoPapel_Click(object sender, EventArgs e)
        {
            Jogar(1);
        }

        // Jogador escolheu PEDRA
        private void picMaoPedra_Click(object sender, EventArgs e)
        {
            Jogar(2);
        }

        // Jogador escolheu TESOURA
        private void picMaoTesoura_Click(object sender, EventArgs e)
        {
            Jogar(3);
        }

        // Inicia o pensamento do computador
        private void Jogar(int jogador)
        {
            jogadaEscolhida = jogador;

            // Coloca todas as imagens em preto e branco
            picMaoPapel.Image = papelCinza;
            picMaoPedra.Image = pedraCinza;
            picMaoTesoura.Image = tesouraCinza;

            // Bloqueia os cliques
            picMaoPapel.Enabled = false;
            picMaoPedra.Enabled = false;
            picMaoTesoura.Enabled = false;

            // Esconde o resultado anterior
            picComputador.Visible = false;
            pictureBox8.Visible = false;

            // Mostra a animação de espera
            picAnimacao.Visible = true;

            // Inicia o Timer
            timerPensamento.Start();
        }

        // Quando o computador terminar de pensar
        private void timerPensamento_Tick(object sender, EventArgs e)
        {
            timerPensamento.Stop();

            // Volta as imagens para as cores originais
            RestaurarImagens();

            // Libera os cliques
            picMaoPapel.Enabled = true;
            picMaoPedra.Enabled = true;
            picMaoTesoura.Enabled = true;

            // Executa a rodada
            ExecutarRodada(jogadaEscolhida);
        }

        // Restaura as imagens coloridas
        private void RestaurarImagens()
        {
            picMaoPapel.Image = imagemPapelOriginal;
            picMaoPedra.Image = imagemPedraOriginal;
            picMaoTesoura.Image = imagemTesouraOriginal;
        }

        // Transforma a imagem em preto e branco
        private Bitmap PretoEBranco(Image imagem)
        {
            Bitmap original = new Bitmap(imagem);
            Bitmap resultado = new Bitmap(original.Width, original.Height);

            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                {
                    Color cor = original.GetPixel(x, y);

                    int cinza = (cor.R + cor.G + cor.B) / 3;

                    resultado.SetPixel(x, y,
                        Color.FromArgb(cor.A, cinza, cinza, cinza));
                }
            }

            original.Dispose();
            return resultado;
        }

        // Executa a rodada
        private void ExecutarRodada(int jogador)
        {
            // Computador escolhe uma jogada
            int computador = gerador.Next(1, 4);

            // Mostra as imagens das jogadas
            picJogador.Image = ImagemJogador(jogador);
            picComputador.Image = ImagemComputador(computador);

            picComputador.Visible = true;
            picAnimacao.Visible = false;

            string resultado = "";

            // Verifica quem ganhou
            if (jogador == computador)
            {
                resultado = "Empate";
            }
            else if (jogador == 1 && computador == 2 ||
                     jogador == 2 && computador == 3 ||
                     jogador == 3 && computador == 1)
            {
                resultado = "Vitória";
                vitoriasJogador++;
            }
            else
            {
                resultado = "Derrota";
                vitoriasComputador++;
            }

            // Mostra a imagem do resultado
            pictureBox8.Image = ImagemResultado(resultado);
            pictureBox8.Visible = true;

            // Atualiza o placar
            lblScoreJogador.Text = vitoriasJogador.ToString();
            lblScoreComputador.Text = vitoriasComputador.ToString();

            // Atualiza o histórico
            AtualizarHistorico(
                picJogador.Image,
                picComputador.Image,
                resultado
            );
        }

        // Imagem do jogador
        private Image ImagemJogador(int jogada)
        {
            if (jogada == 1)
            {
                return Properties.Resources.papel_jogador;
            }
            else if (jogada == 2)
            {
                return Properties.Resources.pedra_jogador;
            }
            else
            {
                return Properties.Resources.tesoura_jogador;
            }
        }

        // Imagem do computador
        private Image ImagemComputador(int jogada)
        {
            if (jogada == 1)
            {
                return Properties.Resources.papel_computador;
            }
            else if (jogada == 2)
            {
                return Properties.Resources.pedra_computador;
            }
            else
            {
                return Properties.Resources.tesoura_computador;
            }
        }

        // Imagem do resultado
        private Image ImagemResultado(string resultado)
        {
            if (resultado == "Vitória")
            {
                return Properties.Resources.ganhou;
            }
            else if (resultado == "Derrota")
            {
                return Properties.Resources.perdeu;
            }
            else
            {
                return Properties.Resources.empate;
            }
        }

        // Atualiza o histórico das últimas 5 rodadas
        private void AtualizarHistorico(Image jogador, Image computador, string resultado)
        {
            picJogador5.Image = picJogador4.Image;
            picComputador5.Image = picComputador4.Image;
            lblResultado5.Text = lblResultado4.Text;

            picJogador4.Image = picJogador3.Image;
            picComputador4.Image = picComputador3.Image;
            lblResultado4.Text = lblResultado3.Text;

            picJogador3.Image = picJogador2.Image;
            picComputador3.Image = picComputador2.Image;
            lblResultado3.Text = lblResultado2.Text;

            picJogador2.Image = picJogador1.Image;
            picComputador2.Image = picComputador1.Image;
            lblResultado2.Text = lblResultado1.Text;

            picJogador1.Image = jogador;
            picComputador1.Image = computador;
            lblResultado1.Text = resultado;
        }

        // Botão limpar placar
        private void btnLimparPlacar_Click(object sender, EventArgs e)
        {
            // Para o Timer caso esteja pensando
            timerPensamento.Stop();

            // Libera os cliques
            picMaoPapel.Enabled = true;
            picMaoPedra.Enabled = true;
            picMaoTesoura.Enabled = true;

            // Restaura as imagens coloridas
            RestaurarImagens();

            // Zera a pontuação
            vitoriasJogador = 0;
            vitoriasComputador = 0;

            lblScoreJogador.Text = "0";
            lblScoreComputador.Text = "0";

            // Limpa as imagens principais
            picJogador.Image = null;
            picComputador.Image = null;
            picComputador.Visible = false;

            pictureBox8.Image = null;
            pictureBox8.Visible = false;

            picAnimacao.Visible = true;

            // Limpa o histórico
            picJogador1.Image = null;
            picJogador2.Image = null;
            picJogador3.Image = null;
            picJogador4.Image = null;
            picJogador5.Image = null;

            picComputador1.Image = null;
            picComputador2.Image = null;
            picComputador3.Image = null;
            picComputador4.Image = null;
            picComputador5.Image = null;

            lblResultado1.Text = "";
            lblResultado2.Text = "";
            lblResultado3.Text = "";
            lblResultado4.Text = "";
            lblResultado5.Text = "";
        }

        // Botão sair
        private void btnNovaPartida_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
