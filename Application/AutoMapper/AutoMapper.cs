using AutoMapper;
using Domain.Models;
using Domain.Objects.Dto_s.User;
using Domain.Objects.Requests.User;
using Domain.Objects.Responses.Asset;
using Domain.Objects.Responses.Card;
using Domain.Utils.Helpers;

namespace Application.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            UserMap();
            BoardMap();
            CardMap();
        }

        #region User

        private void UserMap()
        {
            CreateMap<SaveUserRequest, User>()
               .ForMember(u => u.InsertedAt, opts => opts.MapFrom(u => DateTime.Now));

            CreateMap<User, UserResultsResponse>()
               .ForMember(u => u.Document, opts => opts.MapFrom(u => u.Document.ToDocument()));

            CreateMap<User, UserAuthInfoDto>();
        }

        #endregion

        #region Board

        private void BoardMap()
        {
            CreateMap<SaveBoardRequest, Board>()
               .ForMember(b => b.InsertedAt, opts => opts.MapFrom(s => DateTime.Now));

            CreateMap<UpdateBoardRequest, Board>();

            CreateMap<Board, GetBoardResultsResponse>();
        }

        #endregion

        #region Card

        private void CardMap()
        {
            CreateMap<Card, GetCardResultsResponse>();
        }

        #endregion
    }
}
