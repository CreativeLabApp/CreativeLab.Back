using MediatR;

namespace CreativeLab.Application.Features.Masterclasses.Commands.IncrementMasterclassViews;

public class IncrementMasterclassViewsCommand : IRequest<int>
{
    public Guid MasterclassId { get; set; }
}
