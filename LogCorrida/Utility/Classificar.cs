using System;
using System.Collections.Generic;
using System.Linq;

using LogCorrida.Models;

namespace LogCorrida.Utility
{
    public class Classificar
    {
        private readonly string[] fileRecords;
        private readonly List<Piloto> pilotos;
        
        public Classificar(string fileContent)
        {
            pilotos = new List<Piloto>();
            fileRecords = fileContent.Split("\r\n");

            ProcessLog();
        }

        public List<Classificacao> ObterClassificacao()
        {
            List<Classificacao> classificacoes = new();

            foreach (var piloto in pilotos)
            {
                Classificacao classificacao = new()
                {
                    Codigo = piloto.Codigo,
                    Nome = piloto.Nome,
                    VoltasCompletadas = piloto.Voltas.Count().ToString(),
                    TempoTotal = CalculaTempo(piloto.Voltas)
                };

                classificacoes.Add(classificacao);
            }

            return classificacoes.OrderBy(x => x.TempoTotal).ToList();
        }

        private void ProcessLog()
        {
            bool isFirst = true;
            Piloto piloto = null;
            List<Corrida> listaCorrida = null;

            listaCorrida = fileRecords.Where((val, idx) => idx != 0)
                            .Select(x => x.Split(';'))
                            .Select(c => new Corrida()
                            {
                                Hora = c[0],
                                Piloto = c[1],
                                Volta = c[2],
                                TempoVolta = c[3],
                                VelocidadeMedia = c[4]
                            })
                            .OrderBy(x => x.Piloto)
                            .ToList();

            foreach (var corrida in listaCorrida)
            {
                string[] CodigoNome = corrida.Piloto.Split('-');

                string codigo = CodigoNome[0].Trim();
                string nome = CodigoNome[1].Trim();

                if (!isFirst && piloto.Codigo != codigo)
                {
                    isFirst = true;
                    pilotos.Add(piloto);
                }

                if (isFirst)
                {
                    isFirst = false;
                    piloto = new Piloto
                    {
                        Codigo = codigo,
                        Nome = nome
                    };
                }

                piloto.Voltas.Add(new Volta
                {
                    NumeroVolta = int.Parse(corrida.Volta),
                    TempoVolta = corrida.TempoVolta,
                    VelocidadeMedia = corrida.VelocidadeMedia,
                    Hora = corrida.Hora
                });
            }

            pilotos.Add(piloto);
        }

        private string CalculaTempo(List<Volta> voltas)
        {
            List<long> times = new();

            foreach (var volta in voltas)
            {
                var temp = volta.TempoVolta.Replace(':', '.').Split('.');

                uint minutes = uint.Parse(temp[0]);
                uint seconds = uint.Parse(temp[1]);
                uint milisec = uint.Parse(temp[2]);

                long milis = (minutes * 60000) + (seconds * 1000) + milisec;
                
                times.Add(milis);
            }

            var miliseconds = times.Sum();

            var minute = miliseconds / 60000;
            miliseconds %= 60000;

            var second = miliseconds / 1000;
            miliseconds %= 1000;

            return String.Format("{0}:{1}.{2}", minute, second, miliseconds);
        }
    }
}
