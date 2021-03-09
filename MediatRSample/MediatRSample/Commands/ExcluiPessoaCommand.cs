using MediatR;

namespace MediatRSample.Commands
{
    public class ExcluiPessoaCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}