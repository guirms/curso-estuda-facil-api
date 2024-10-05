using Domain.Interfaces.Externals;
using Domain.Objects.Dto_s.Card;
using Domain.Utils.Helpers;
using Infra.CrossCutting.Externals.Bases;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infra.CrossCutting.Externals
{
    public class PyAIExternal(IHttpClientFactory client, IConfiguration configuration) : BaseExternal(configuration.GetTag("PyAiIp"), client), IPyAIExternal
    {
        public async Task<IEnumerable<CardData>> GenerateBoard(string theme, int daysUntilExam)
        {
            try
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["use_openai"] = "true",
                    ["theme"] = theme,
                    ["days_until_exam"] = daysUntilExam.ToString()
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                IEnumerable<CardData>? response = null;

                var count = 0;

                while (response == null && count < 5)
                {
                    try
                    {
                        var result = await Get("generateBoard", parameters, false);

                        response = JsonSerializer.Deserialize<IEnumerable<CardData>>(result!, jsonOptions);
                    }
                    catch
                    {
                        response = null;
                    }

                    count++;
                }

                if (response == null)
                    throw new InvalidOperationException("Erro ao criar board. Por favor, modifique o tema e tente novamente");

                return response;
            }
            catch
            {
                throw new HttpRequestException("Erro ao se comunicar com serviço externo");
            }
        }
    }
}
