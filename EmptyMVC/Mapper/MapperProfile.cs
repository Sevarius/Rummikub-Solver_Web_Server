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

            CreateMap<CardDto, CardModel>(MemberList.Source);

            CreateMap<CombinationDto, CombinationModel>(MemberList.Source);

            CreateMap<GameDto, GameModel>(MemberList.Source);
        }
    }
}