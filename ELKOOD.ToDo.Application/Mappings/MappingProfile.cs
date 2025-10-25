using AutoMapper;
using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Application.DTOs.ToDo;
using ELKOOD.ToDo.Application.DTOs.User;

namespace ELKOOD.ToDo.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ToDo mappings
            CreateMap<ToDoItem, ToDoItemDto>();
            CreateMap<CreateToDoItemDto, ToDoItem>();
            CreateMap<UpdateToDoItemDto, ToDoItem>();

            // User mappings
            CreateMap<User, UserDto>();
        }
    }
}