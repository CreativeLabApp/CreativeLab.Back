using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Users.Queries.GetUserDetails;

public class UserDetailsVm : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int MasterclassesCount { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<User, UserDetailsVm>()
            .ForMember(d => d.MasterclassesCount, opt => opt.MapFrom(s => s.CreatedMasterclasses.Count));
}
