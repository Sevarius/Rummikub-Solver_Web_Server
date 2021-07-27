using AutoMapper;
using DTOModel;
using MechanicsModel;

namespace Mapper
{
    public sealed class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<string, CardColor>().ConvertUsing<StringCardColorConverter>();

            CreateMap<CardDto, CardModel>(MemberList.Source).ReverseMap();

            CreateMap<CombinationDto, CombinationModel>(MemberList.Source).ReverseMap();

            CreateMap<GameDto, GameModel>(MemberList.Source).ReverseMap();
        }
    }
}