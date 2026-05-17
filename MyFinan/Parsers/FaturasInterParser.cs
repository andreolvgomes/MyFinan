using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using MyFinan.Models.Dtos;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyFinan.Parsers
{
    public class FaturasInterParser
    {
        public async Task<List<TransacaoResponse>> Extrair(MemoryStream stream)
        {
            var listaTransacoes = new List<TransacaoResponse>();

            var regexTransacao = new Regex(@"(\d{2}\s+de\s+[a-zA-Z]{3}\.?\s+\d{4})\s+(.+?)\s*(-?\s*\+?\s*R\$\s*[\d.,]+)", RegexOptions.IgnoreCase);

            using (var pdfReader = new PdfReader(stream))
            using (var pdfDocument = new PdfDocument(pdfReader))
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    var estrategia = new LocationTextExtractionStrategy();
                    var textoPagina = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), estrategia);

                    textoPagina = Regex.Replace(textoPagina, @"\s+", " ");

                    var matches = regexTransacao.Matches(textoPagina);

                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            string beneficiario = match.Groups[2].Value.Trim();

                            if (beneficiario.EndsWith("-"))
                                beneficiario = beneficiario.Substring(0, beneficiario.Length - 1).Trim();

                            listaTransacoes.Add(new TransacaoResponse
                            {
                                Data = ConverterDataFatura(match.Groups[1].Value.Trim()),
                                Beneficiario = beneficiario,
                                Valor = ConverterValorFatura(match.Groups[3].Value.Trim())
                            });
                        }
                    }
                }
            }

            return listaTransacoes;
        }

        public decimal ConverterValorFatura(string valorTexto)
        {
            if (string.IsNullOrWhiteSpace(valorTexto))
                return 0;

            var textoLimpo = Regex.Replace(valorTexto, @"[^\d,.-]", "");
            var culturaBR = new CultureInfo("pt-BR");

            if (decimal.TryParse(textoLimpo, NumberStyles.Any, culturaBR, out decimal resultado))
                return resultado;

            return 0;
        }

        public DateTime? ConverterDataFatura(string dataTexto)
        {
            if (string.IsNullOrWhiteSpace(dataTexto))
                return null;

            try
            {
                var dataLimpa = dataTexto.Replace(".", "").ToLower();
                var partes = dataLimpa.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length < 4) return null;

                var diaStr = partes[0];
                var mesStr = partes[2];
                var anoStr = partes[3];

                // 2. Dicionário manual para evitar depender da internacionalização do Linux da AWS
                int mes = mesStr switch
                {
                    "jan" => 1,
                    "fev" => 2,
                    "mar" => 3,
                    "abr" => 4,
                    "mai" => 5,
                    "jun" => 6,
                    "jul" => 7,
                    "ago" => 8,
                    "set" => 9,
                    "out" => 10,
                    "nov" => 11,
                    "dez" => 12,
                    _ => 0
                };

                if (mes == 0) return null;

                int dia = int.Parse(diaStr);
                int ano = int.Parse(anoStr);

                return new DateTime(ano, mes, dia);
            }
            catch
            {
                return null;
            }
        }
    }
}
