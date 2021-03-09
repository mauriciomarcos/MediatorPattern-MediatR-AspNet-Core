using MediatR;
using MediatRSample.Commands;
using MediatRSample.Models;
using MediatRSample.Models.Interfaces;
using MediatRSample.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRSample.Handlers
{
    public class ExcluiPessoaCommandHandler : IRequestHandler<ExcluiPessoaCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Pessoa> _repository;

        public ExcluiPessoaCommandHandler(IMediator mediator, IRepository<Pessoa> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<string> Handle(ExcluiPessoaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.Delete(request.Id);
                await _mediator.Publish(new PessoaExcluidaNotification 
                { 
                    Id = request.Id,
                    IsEfetivado = true
                });

                return await Task.FromResult("Pessoa excluída com sucesso!");
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new PessoaExcluidaNotification
                {
                    Id = request.Id,
                    IsEfetivado = false
                });

                await _mediator.Publish(new ErrorNotification { Error = ex.Message, StackTraceError = ex.StackTrace});

                return await Task.FromResult("Falha na tentativa de exclusão.");
            }
        }
    }
}