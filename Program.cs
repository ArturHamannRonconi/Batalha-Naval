using System;

namespace BatalhaNaval
{
    class Casa
    {
        public byte Linha
        {
            get => Linha;
            set
            {
                if(value <= 10)
                    Linha = value;
            }
        }
        public byte Coluna
        {
            get => Coluna;
            set
            {
                if(value <= 10)
                    Coluna = value;
            }
        }
        public NavioMaisProximo navioMaisProximo; 
        
        public Casa(byte linha, byte coluna, NavioMaisProximo navioMaisProximo)
        {
            this.Linha = linha;
            this.Coluna = coluna;
            this.navioMaisProximo = navioMaisProximo;
        }
    }
    enum NavioMaisProximo
    {
        AQUI,
        UMA_CASA,
        DUAS_CASAS,
        TRES_CASAS,
        QUATRO_OU_MAIS_CASAS
    }
    
    class Tabuleiro
    {
        private const byte MAXIMO_NAVIOS = 10;
        public Casa[,] casas = new Casa[10, 10];

        public void arrumarTabuleiro()
        {
            setLinhasColunas();
            setNavios();
            procurarNaviosProximos();
        }

        private void setLinhasColunas()
        {
            for(byte linha = 0; linha < casas.GetLength(0); linha++)
            {
                for(byte coluna = 0; coluna < casas.GetLength(1); coluna++)
                {
                    casas[linha, coluna] = new Casa(
                        linha, coluna, 
                        NavioMaisProximo.QUATRO_OU_MAIS_CASAS
                    );
                }
            }
        }
        private void setNavios()
        {
            for(byte navio = 1; navio <= MAXIMO_NAVIOS; navio++)
            {
                Console.Write($"Digite a coluna para o navio {navio}: ");
                var coluna = byte.Parse(Console.ReadLine());

                Console.Write($"Digite a linha para o navio {navio}: ");
                var linha = byte.Parse(Console.ReadLine());

                casas[linha, coluna] = new Casa(linha, coluna, NavioMaisProximo.AQUI);
            }
        }
        private void procurarNaviosProximos()
        {
            foreach (var casa in casas)
            {
                if(casa.navioMaisProximo != NavioMaisProximo.AQUI)
                {
                    var NavioComUmaCasaProxima = hasNavioProximo(casa, 1);
                    var NavioComDuasCasasProxima = hasNavioProximo(casa, 2);
                    var NavioComTresCasasProxima = hasNavioProximo(casa, 3);

                    if(NavioComUmaCasaProxima)
                        casa.navioMaisProximo = NavioMaisProximo.UMA_CASA;
                    else if(NavioComDuasCasasProxima)
                        casa.navioMaisProximo = NavioMaisProximo.DUAS_CASAS;
                    else if(NavioComTresCasasProxima)
                        casa.navioMaisProximo = NavioMaisProximo.TRES_CASAS;
                    else
                        casa.navioMaisProximo = NavioMaisProximo.QUATRO_OU_MAIS_CASAS;
                }
            }            
        }
        private bool hasNavioProximo(Casa casa, byte distanciaProxima)
        {
            // Não consegui continuar com o clean code, o código verifica se a há navios na distancia pedida ao redor da casa enviada.
            return 
                (hasCasaProxima((byte) (casa.Linha + distanciaProxima)) && casas[casa.Linha + distanciaProxima, casa.Coluna].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Linha - distanciaProxima)) && casas[casa.Linha - distanciaProxima, casa.Coluna].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && casas[casa.Linha, casa.Coluna + distanciaProxima].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Coluna - distanciaProxima)) && casas[casa.Linha, casa.Coluna - distanciaProxima].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Coluna - distanciaProxima)) && hasCasaProxima((byte) (casa.Linha - distanciaProxima)) && casas[casa.Linha - distanciaProxima, casa.Coluna - distanciaProxima].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && hasCasaProxima((byte) (casa.Linha + distanciaProxima)) && casas[casa.Linha + distanciaProxima, casa.Coluna + distanciaProxima].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && hasCasaProxima((byte) (casa.Linha + distanciaProxima)) && casas[casa.Linha + distanciaProxima, casa.Coluna - distanciaProxima].navioMaisProximo == NavioMaisProximo.AQUI) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && hasCasaProxima((byte) (casa.Linha - distanciaProxima)) && casas[casa.Linha - distanciaProxima, casa.Coluna + distanciaProxima].navioMaisProximo == NavioMaisProximo.AQUI);
        }
        private bool hasCasaProxima(byte coordenada)
        {
            return coordenada <= 9 && coordenada >= 0;
        }
    }

    class Jogador
    {
        private int Pontos { get; set; } = 0;
        public string Nome { get; set; }
        public Tabuleiro tabuleiro;
        
        private Jogador(string nome, Tabuleiro tabuleiro)
        {
            this.Nome = nome;
            this.tabuleiro = tabuleiro;
        }

        public static Jogador criarJogador()
        {
            Console.Write("Digite seu nome: ");
            
            var nome = Console.ReadLine();
            var tabuleiro = new Tabuleiro();

            return new Jogador(nome, tabuleiro);
        }

        public (byte linha, byte coluna) plantarBomba()
        {            
            Console.Write($"Digite a coluna para explodir uma bomba: ");
            var coluna = byte.Parse(Console.ReadLine());

            Console.Write($"Digite a linha para explodir uma bomba: ");
            var linha = byte.Parse(Console.ReadLine());

            return (linha, coluna);
        }

        public void adicionarCemPontos()
        {
            Pontos += 100;
        }

        public void adicionarSetentaPontos()
        {
            Pontos += 70;
        }

        public int getPontos()
        {
            return Pontos;
        }
    }

    class Jogo
    {
        private const byte MAXIMA_BOMBAS = 15;
        public Jogador[] jogadores;

        public Jogo() => start();

        private void start()
        {
            jogadores = new Jogador[2] {
                Jogador.criarJogador(),
                Jogador.criarJogador()
            };

            Array.ForEach<Jogador>(jogadores, arrumarTabuleiros);
            plantarBombas(jogadores[0], jogadores[1]);
        }

        private void arrumarTabuleiros(Jogador jogador)
        {
            Console.Clear();
            Console.WriteLine($"|O jogador {jogador.Nome} deve escolher as coordenadas para seus navios|");
            jogador.tabuleiro.arrumarTabuleiro();
        
        }
        private void plantarBombas(Jogador jogador01, Jogador jogador02)
        {
            Console.Clear();
            Console.WriteLine($"|O jogador {jogador01.Nome} deve escolher as coordenadas para explodir os navios inimigos|");
            
            for(int bomba = 0; bomba < MAXIMA_BOMBAS; bomba++)
            {
                var bombaJogador01 = jogador01.plantarBomba();
                somarPontos(bombaJogador01, jogador01, jogador02);
            }

            for(int bomba = 0; bomba < MAXIMA_BOMBAS; bomba++)
            {
                var bombaJogador02 = jogador01.plantarBomba();
                somarPontos(bombaJogador02, jogador02, jogador01);
            }


            var vencedor = calcularPontuacaoVencedor(jogador01, jogador02);
            mostrarVencedor(vencedor);
        }
        private void somarPontos((byte linha, byte coluna) bomba, Jogador jogador01, Jogador jogador02)
        {
            var navioInimigoMaisProximo = jogador02
                .tabuleiro
                .casas[bomba.linha, bomba.coluna]
                .navioMaisProximo;

            if(navioInimigoMaisProximo == NavioMaisProximo.AQUI) {
                jogador01.adicionarCemPontos();
            } else if(navioInimigoMaisProximo == NavioMaisProximo.UMA_CASA){
                Console.WriteLine("Errou por 1!");
            } else if(navioInimigoMaisProximo == NavioMaisProximo.DUAS_CASAS) {
                jogador02.adicionarSetentaPontos();
                Console.WriteLine("Errou por 2!");
            } else if(navioInimigoMaisProximo == NavioMaisProximo.TRES_CASAS) {
                jogador02.adicionarSetentaPontos();
                Console.WriteLine("Errou por 3!");
            } else {
                jogador02.adicionarSetentaPontos();
                Console.WriteLine("Errou por muitas!");
            }
        }      
        private Jogador calcularPontuacaoVencedor(Jogador jogador01, Jogador jogador02)
        {
            return (jogador01.getPontos() > jogador02.getPontos())
                ? jogador01 
                : jogador02; 
        }
        private void mostrarVencedor(Jogador vencedor)
        {
            Console.Clear();
            Console.WriteLine($"O Vencedor foi {vencedor.Nome} com {vencedor.getPontos()} pontos.");
        }
    }

    class Program
    {
        public Program() => new Jogo();
        public static void Main(string[] args) => new Program();
    }
}