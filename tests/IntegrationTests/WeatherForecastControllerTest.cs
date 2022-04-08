using IntegrationTests.Utilities;
using Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class WeatherForecastControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Reporto_Success()
        {
            //arrange
            var shouldBe = DbHelper.GetTestForecasts().Select(v 
                => $"<td>{v.Date}</td> <td>{v.TemperatureC}</td> <td>{v.TemperatureF}</td> <td>{v.Summary}</td>")
                .ToArray().ToString();

            //act
            var response = await client.GetAsync("/weather/reporto");
            var json = await response.Content.ReadAsStringAsync();
            var table = json.Substring(json.IndexOf("<table"), json.IndexOf("</table>") - json.IndexOf("<table"));
            var dataRows = table.Split($"</tr>{Environment.NewLine}").Skip(1)
                .Where(v => !string.IsNullOrWhiteSpace(v));
            var formattedData = dataRows
                .Select(v 
                    => string.Join(" ", v.Split(Environment.NewLine).Skip(1).Select(s => s.Trim())))
                .ToArray().ToString();

            //assert
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(shouldBe, formattedData);

        }
    }
}
