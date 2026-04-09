using AutoMapper;
using CreativeLab.Application.Features.Categories.Commands.CreateCategory;
using CreativeLab.Application.Features.Categories.Commands.UpdateCategory;
using CreativeLab.Application.Features.ChatParticipants.Commands.AddChatParticipant;
using CreativeLab.Application.Features.ChatParticipants.Commands.RemoveChatParticipant;
using CreativeLab.Application.Features.Chats.Commands.CreateChat;
using CreativeLab.Application.Features.Masterclasses.Commands.CreateMasterclass;
using CreativeLab.Application.Features.Masterclasses.Commands.UpdateMasterclass;
using CreativeLab.Application.Features.Messages.Commands.CreateMessage;
using CreativeLab.Application.Features.ProductMaterials.Commands.CreateProductMaterial;
using CreativeLab.Application.Features.ProductMaterials.Commands.UpdateProductMaterial;
using CreativeLab.Application.Features.Products.Commands.CreateProduct;
using CreativeLab.Application.Features.Products.Commands.UpdateProduct;
using CreativeLab.Application.Features.Tags.Commands.CreateTag;
using CreativeLab.Application.Features.Tags.Commands.UpdateTag;
using CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteMasterclass;
using CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteProduct;
using CreativeLab.Application.Features.UserFavorites.Commands.RemoveFavoriteMasterclass;
using CreativeLab.Application.Features.UserFavorites.Commands.RemoveFavoriteProduct;
using CreativeLab.WebApi.Dto;

namespace CreativeLab.WebApi.Mappings;

public class WebApiMappingProfile : Profile
{
    public WebApiMappingProfile()
    {
        CreateMap<CreateTagDto, CreateTagCommand>();
        CreateMap<UpdateTagDto, UpdateTagCommand>();

        CreateMap<CreateCategoryDto, CreateCategoryCommand>();
        CreateMap<UpdateCategoryDto, UpdateCategoryCommand>();

        CreateMap<CreateProductMaterialDto, CreateProductMaterialCommand>();
        CreateMap<UpdateProductMaterialDto, UpdateProductMaterialCommand>();

        CreateMap<CreateMasterclassDto, CreateMasterclassCommand>();
        CreateMap<UpdateMasterclassDto, UpdateMasterclassCommand>();

        CreateMap<CreateProductDto, CreateProductCommand>();
        CreateMap<UpdateProductDto, UpdateProductCommand>();

        CreateMap<CreateChatDto, CreateChatCommand>();
        CreateMap<AddChatParticipantDto, AddChatParticipantCommand>();
        CreateMap<RemoveChatParticipantDto, RemoveChatParticipantCommand>();

        CreateMap<CreateMessageDto, CreateMessageCommand>();

        CreateMap<AddFavoriteMasterclassDto, AddFavoriteMasterclassCommand>();
        CreateMap<RemoveFavoriteMasterclassDto, RemoveFavoriteMasterclassCommand>();
        CreateMap<AddFavoriteProductDto, AddFavoriteProductCommand>();
        CreateMap<RemoveFavoriteProductDto, RemoveFavoriteProductCommand>();
    }
}
