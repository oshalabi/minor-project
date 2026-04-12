using Domain.Entities;
using Ration.Model;

namespace Ration.Commands;

public class AddFeedTypeMultiple
{
    public required ICollection<FeedTypeDTO> FeedTypes { get; set; }
    public bool IsEnergy  { get; set; } = false;
}