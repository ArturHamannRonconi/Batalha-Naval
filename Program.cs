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
        public bool hasNavio = false;
        public string Warning { get; set; }
        
        public Casa(byte linha, byte coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
        }
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
                    casas[linha, coluna] = new Casa(linha, coluna);
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

                casas[linha, coluna].hasNavio = true;
            }
        }
        private void procurarNaviosProximos()
        {
            foreach (var casa in casas)
            {
                if(!casa.hasNavio)
                {
                    var NavioComUmaCasaProxima = hasNavioProximo(casa, 1);
                    var NavioComDuasCasasProxima = hasNavioProximo(casa, 2);
                    var NavioComTresCasasProxima = hasNavioProximo(casa, 3);

                    if(NavioComUmaCasaProxima)
                        casa.Warning = "Errou por 1!";
                    else if(NavioComDuasCasasProxima)
                        casa.Warning = "Errou por 2!";
                    else if(NavioComTresCasasProxima)
                        casa.Warning = "Errou por 3!";
                    else
                        casa.Warning = "Errou por muitas!";
                }
            }            
        }
        private bool hasNavioProximo(Casa casa, byte distanciaProxima)
        {
            // Não consegui continuar com o clean code, o código verifica se a há navios na distancia pedida ao redor da casa enviada.
            return 
                (hasCasaProxima((byte) (casa.Linha + distanciaProxima)) && casas[casa.Linha + distanciaProxima, casa.Coluna].hasNavio) ||
                (hasCasaProxima((byte) (casa.Linha - distanciaProxima)) && casas[casa.Linha - distanciaProxima, casa.Coluna].hasNavio) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && casas[casa.Linha, casa.Coluna + distanciaProxima].hasNavio) ||
                (hasCasaProxima((byte) (casa.Coluna - distanciaProxima)) && casas[casa.Linha, casa.Coluna - distanciaProxima].hasNavio) ||
                (hasCasaProxima((byte) (casa.Coluna - distanciaProxima)) && hasCasaProxima((byte) (casa.Linha - distanciaProxima)) && casas[casa.Linha - distanciaProxima, casa.Coluna - distanciaProxima].hasNavio) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && hasCasaProxima((byte) (casa.Linha + distanciaProxima)) && casas[casa.Linha + distanciaProxima, casa.Coluna + distanciaProxima].hasNavio) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && hasCasaProxima((byte) (casa.Linha + distanciaProxima)) && casas[casa.Linha + distanciaProxima, casa.Coluna - distanciaProxima].hasNavio) ||
                (hasCasaProxima((byte) (casa.Coluna + distanciaProxima)) && hasCasaProxima((byte) (casa.Linha - distanciaProxima)) && casas[casa.Linha - distanciaProxima, casa.Coluna + distanciaProxima].hasNavio);
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
                .casas[bomba.linha, bomba.coluna];

            if(navioInimigoMaisProximo.hasNavio) {
                Console.WriteLine($"Acertou um návio +100 pontos para o jogador {jogador01.Nome}");
                jogador01.adicionarCemPontos(); 
            } else {
                jogador02.adicionarSetentaPontos();
                Console.WriteLine(navioInimigoMaisProximo.Warning);
                Console.WriteLine($"Errou +70 pontos para o jogador {jogador02.Nome}");
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