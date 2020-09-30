using AutoMapper;
using SocialAPI.DTOs;
using SocialAPI.DTOs.User;
using SocialAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<PostUserDTO, User>();

            CreateMap<User, CreatedUserDTO>();

            CreateMap<User, AuthenticatedUserDTO>();


            //Post
            var fromPostToReturnedpost = CreateMap<Post, ReturnedPostDTO>();
            fromPostToReturnedpost.
                ForMember(dest => dest.Categories,
                           opt => opt.MapFrom(src => src.PostCategories
                                                .Select(postCategory =>
                                                 new CategoryDTO { Id = postCategory.Category.Id, Name = postCategory.Category.Name })));

            CreateMap<ReturnedPostDTO, Post>();
            CreateMap<PostDTO, Post>();

            //Comment
            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();

            //Categories
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();

            
        }
    }
}
