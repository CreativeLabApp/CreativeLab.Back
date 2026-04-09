using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Tags.Commands.UpdateTag;

public class UpdateTagCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateTagCommand, Tag>
{
    public async Task<Tag> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await dbContext.Tags
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Tag with this Id does not exist");

        tag.Name = request.Name;
        tag.Color = request.Color;

        await dbContext.SaveChangesAsync(cancellationToken);

        return tag;
    }
}
