using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoDaVelha
{
    internal class JogoDaVelha
    {
        static void Main(string[] args)
        {
            string[,] matriz = new string[3, 3];
            string turno;
            List<string> indexNumeros = new List<string>();
            int index = 1;
            int tentativas = 0;

            Console.WriteLine("-------------------------------");
            Console.WriteLine("  BEM-VINDO AO JOGO DA VELHA   ");
            Console.WriteLine("-------------------------------");

            
            Console.WriteLine("\nQuem deve começar o jogo?");
            Console.WriteLine("[1] Jogador (X)");
            Console.WriteLine("[2] Máquina (O)");
            Console.Write("Escolha uma opção (1 ou 2): ");
            string escolha = Console.ReadLine();

            if (escolha == "1")
            {
                turno = "X";
            }
            else if (escolha == "2")
            {
                turno = "O";
            }
            else
            {
                Console.WriteLine("Opção inválida! O jogador (X) começará por padrão.");
                turno = "X";
            }

            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matriz[i, j] = index.ToString();
                    indexNumeros.Add(index.ToString());
                    index++;
                }
            }

            while (tentativas < 9)
            {
                
                ExibirTabuleiro(matriz);

                if (turno == "X")
                {
                    
                    string jogada;
                    do
                    {
                        Console.Write($"\nVocê quer jogar [{turno}] em qual posição?: ");
                        jogada = Console.ReadLine();

                        if (!indexNumeros.Contains(jogada))
                        {
                            Console.WriteLine("Jogada Inválida! Tente novamente.");
                        }
                    } while (!indexNumeros.Contains(jogada));

                    
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (matriz[i, j] == jogada)
                            {
                                matriz[i, j] = turno;
                                indexNumeros.Remove(jogada);
                            }
                        }
                    }
                }
                else
                {
                    
                    var melhorJogada = ObterMelhorJogadaMinimax(matriz, "O", "X");
                    matriz[melhorJogada.Item1, melhorJogada.Item2] = turno;
                    indexNumeros.Remove(((melhorJogada.Item1 * 3) + melhorJogada.Item2 + 1).ToString());
                    Console.WriteLine($"A máquina escolheu a posição [{(melhorJogada.Item1 * 3) + melhorJogada.Item2 + 1}].");
                }

                tentativas++;

               
                if (VerificarVitoria(matriz))
                {
                    ExibirTabuleiro(matriz);
                    Console.WriteLine("\n--------------");
                    Console.WriteLine("FIM DE JOGO!!!");
                    Console.WriteLine("--------------");
                    Console.WriteLine($"\nParabéns, quem ganhou foi: [{turno}]");
                    break;
                }

                
                if (tentativas == 9)
                {
                    ExibirTabuleiro(matriz);
                    Console.WriteLine("\n--------------");
                    Console.WriteLine("FIM DE JOGO!!!");
                    Console.WriteLine("--------------");
                    Console.WriteLine("\nEste jogo deu Velha. Ninguém Ganhou!");
                    break;
                }

                
                turno = (turno == "X") ? "O" : "X";

                Console.Clear();
            }

            Console.ReadLine();
        }

        
        static void ExibirTabuleiro(string[,] matriz)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"[{matriz[i, j]}] ");
                }
                Console.WriteLine();
            }
        }

        
        static bool VerificarVitoria(string[,] matriz)
        {
            
            for (int i = 0; i < 3; i++)
            {
                if (matriz[i, 0] == matriz[i, 1] && matriz[i, 1] == matriz[i, 2])
                {
                    return true;
                }
            }

            
            for (int i = 0; i < 3; i++)
            {
                if (matriz[0, i] == matriz[1, i] && matriz[1, i] == matriz[2, i])
                {
                    return true;
                }
            }

            
            if ((matriz[0, 0] == matriz[1, 1] && matriz[1, 1] == matriz[2, 2]) ||
                (matriz[0, 2] == matriz[1, 1] && matriz[1, 1] == matriz[2, 0]))
            {
                return true;
            }

            return false;
        }

        
        static (int, int) ObterMelhorJogadaMinimax(string[,] matriz, string turno, string adversario)
        {
            int melhorValor = int.MinValue;
            (int, int) melhorJogada = (-1, -1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matriz[i, j] != "X" && matriz[i, j] != "O")
                    {
                        string backup = matriz[i, j];
                        matriz[i, j] = turno;

                        int valor = Minimax(matriz, 0, false, turno, adversario);

                        matriz[i, j] = backup;

                        if (valor > melhorValor)
                        {
                            melhorValor = valor;
                            melhorJogada = (i, j);
                        }
                    }
                }
            }

            return melhorJogada;
        }

        
        static int Minimax(string[,] matriz, int profundidade, bool isMaximizing, string turno, string adversario)
        {
            if (VerificarVitoria(matriz))
            {
                return isMaximizing ? -10 : 10;
            }

            if (IsEmpate(matriz))
            {
                return 0;
            }

            if (isMaximizing)
            {
                int melhorValor = int.MinValue;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (matriz[i, j] != "X" && matriz[i, j] != "O")
                        {
                            string backup = matriz[i, j];
                            matriz[i, j] = turno;

                            int valor = Minimax(matriz, profundidade + 1, false, turno, adversario);

                            matriz[i, j] = backup;
                            melhorValor = Math.Max(melhorValor, valor);
                        }
                    }
                }

                return melhorValor;
            }
            else
            {
                int melhorValor = int.MaxValue;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (matriz[i, j] != "X" && matriz[i, j] != "O")
                        {
                            string backup = matriz[i, j];
                            matriz[i, j] = adversario;

                            int valor = Minimax(matriz, profundidade + 1, true, turno, adversario);

                            matriz[i, j] = backup;
                            melhorValor = Math.Min(melhorValor, valor);
                        }
                    }
                }

                return melhorValor;
            }
        }

        
        static bool IsEmpate(string[,] matriz)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matriz[i, j] != "X" && matriz[i, j] != "O")
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}