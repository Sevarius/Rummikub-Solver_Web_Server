using System.Collections.Generic;
using AutoMapper;
using DTOModel;
using Mapper;
using MechanicsModel;
using Xunit;

namespace EmptyMVC.Tests
{
    public class MapperTest
    {
        private readonly IMapper _mapper;
        public MapperTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });
            configuration.CompileMappings();
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MapperValidationTest()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData("1r", "red")]
        [InlineData("j", "joker")]
        [InlineData("13bb", "Black")]
        [InlineData("10y", "y")]
        public void CardMapTest(string cardStr, string dtoColor)
        {
            var converter = new StringToCombinationConverter(CombinationStringFormat.Short);
            var cardModel = converter.StringToCard(cardStr);
            var cardDto = new CardDto
            {
                Color = dtoColor,
                Number = cardModel.Number
            };

            var mappedCardModel = _mapper.Map<CardModel>(cardDto);

            Assert.Equal(cardModel, mappedCardModel);
        }

        [Theory]
        [InlineData("1r 2r 3r", "red", "r", "Red")]
        [InlineData("13bb j j", "Black", "J", "Joker")]
        [InlineData("10y 10y 11y", "Y", "y", "yellow")]
        public void CombinationMapTest(string combination, params string[] dtoColors)
        {
            var converter = new StringToCombinationConverter(CombinationStringFormat.Short);
            var combinationModel = converter.StringToCombination(combination);
            var combinationDto = new CombinationDto
            {
                Cards = new List<CardDto>()
            };
            for (var i = 0; i < dtoColors.Length; i++)
            {
                combinationDto.Cards.Add(new CardDto{Color = dtoColors[i], Number = combinationModel[i].Number});
            }

            var mapperCombinationModel = _mapper.Map<CombinationModel>(combinationDto);

            Assert.Equal(combinationModel, mapperCombinationModel);
        }

        [Fact]
        public void ReverseMapTest()
        {
            var cardModel = new CardModel(CardColor.Black, 5);

            var cardDto = _mapper.Map<CardDto>(cardModel);
        }
    }
}