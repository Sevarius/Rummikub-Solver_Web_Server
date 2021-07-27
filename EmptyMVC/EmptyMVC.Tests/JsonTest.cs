using DTOModel;
using Newtonsoft.Json;
using Xunit;

namespace EmptyMVC.Tests
{
    public class JsonTest
    {
        public JsonTest()
        {

        }

        [Theory]
        [InlineData(@"{""color"":""red"",""number"":4}", "red", 4)]
        public void CardDeserializeTest(string json, string color, int number)
        {
            var cardDto = JsonConvert.DeserializeObject<CardDto>(json);
            Assert.NotNull(cardDto);
            Assert.Equal(color, cardDto.Color);
            Assert.Equal(number, cardDto.Number);
        }

        [Theory]
        [InlineData(
            @"{""table"":[{""cards"":[{""color"":""red"",""number"":1},{""color"":""red"",""number"":2},{""color"":""red"",""number"":3}]}], ""hand"":[{""color"":""red"",""number"":1}]}",
            1, 1)]
        public void GameDeserializeTest(string json, int combNumber, int cardNumber)
        {
            var gameDto = JsonConvert.DeserializeObject<GameDto>(json);
            Assert.NotNull(gameDto);
            Assert.NotNull(gameDto.Table);
            Assert.Collection(gameDto.Table, x=> Assert.NotNull(x.Cards));
            Assert.Equal(combNumber, gameDto.Table.Count);
            Assert.NotNull(gameDto.Hand);
            Assert.Collection(gameDto.Hand, Assert.NotNull);
            Assert.Equal(cardNumber, gameDto.Hand.Count);
        }
    }
}